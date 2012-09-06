using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.ViewModelProperties;
using GasyTek.Lakana.Mvvm.ViewModels;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// Base class from which custom fluent validation engines should derive.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    public abstract class FluentValidationEngine<TViewModel> : ValidationEngineBase where TViewModel : ViewModelBase
    {
        private readonly Parser _parser;
        private readonly TViewModel _viewModelInstance;

        // contains rule definitions for each property
        private readonly Dictionary<string, List<FluentImplementer<TViewModel>>> _propertyRuleCollection;

        // contains the couple (AST, Error message) for each property.
        private readonly Dictionary<string, List<CompiledRule>> _compiledRules;

        /// <summary>
        /// Do not use this constructor, it is intended to be used by the infrastructure only.
        /// </summary>
        internal FluentValidationEngine(TViewModel viewModelInstance, bool buildRuleImmediatly)
        {
            _parser = new Parser();
            _propertyRuleCollection = new Dictionary<string, List<FluentImplementer<TViewModel>>>();
            _compiledRules = new Dictionary<string, List<CompiledRule>>();
            _viewModelInstance = viewModelInstance;

            if (buildRuleImmediatly) BuildRules();
        }

        protected FluentValidationEngine(TViewModel viewModelInstance)
            : this(viewModelInstance, true)
        {
        }

        internal void BuildRules()
        {
            DefineRules();
            EnsureRulesAreValid();
            CompileRules();
        }

        #region Rule definition

        /// <summary>
        /// Define the rules for all properties.
        /// Use <see cref="Property" /> to begin defining rules.
        /// </summary>
        protected abstract void DefineRules();

        /// <summary>
        /// Allows to define rule for one property.
        /// </summary>
        /// <param name="propertyExpression">The expression that specify the property to define rule for.</param>
        /// <remarks>This method will typically called from <see cref="DefineRules"/> method.</remarks>
        protected IFluentVerb<TViewModel> Property(Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            var property = propertyExpression.Compile()(_viewModelInstance);
            if (property == null)
                throw new InvalidOperationException(
                    "Property instance is null. Please verify that you assigned a model to your view model.");

            var propertyName = property.PropertyMetadata.Name;
            var fluentApi = new FluentImplementer<TViewModel>(_viewModelInstance);

            // registers the rule for this property 
            if (!_propertyRuleCollection.ContainsKey(propertyName))
                _propertyRuleCollection.Add(propertyName, new List<FluentImplementer<TViewModel>>());

            _propertyRuleCollection[propertyName].Add(fluentApi);

            return fluentApi.Property(propertyExpression);
        }

        private void EnsureRulesAreValid()
        {
            var allRules = from propertyRule in _propertyRuleCollection
                           from rule in propertyRule.Value
                           select rule;

            allRules.ToList().ForEach(rule =>
                                                 {
                                                     if (rule.IsExpressionValid() == false)
                                                         throw new RuleDefinitionException(
                                                             string.Format("Rule for property [{0}] is incomplete.",
                                                                           rule.Context.OwnerProperty.PropertyMetadata
                                                                               .Name));
                                                 });
        }

        #endregion

        #region Rule interpretation

        private void CompileRules()
        {
            // for each property that have a defined rules 
            foreach (var propertyRule in _propertyRuleCollection)
            {
                // for each rule define on the property
                var ruleCollection = propertyRule.Value;
                foreach (var rule in ruleCollection)
                {
                    // compile the rule to get an abstract syntax tree
                    var ruleId = Guid.NewGuid().ToString();
                    var ast = _parser.Parse(rule.InternalTokens);
                    var concernedProperties = rule.Context.Properties;
                    var errorMessage = rule.Context.Message;

                    // attach the compiled rule to all concerned properties
                    foreach (var property in concernedProperties)
                    {
                        var propertyName = property.PropertyMetadata.Name;

                        if (_compiledRules.ContainsKey(propertyName) == false)
                            _compiledRules.Add(propertyName, new List<CompiledRule>());

                        _compiledRules[propertyName].Add(new CompiledRule(ruleId, concernedProperties, ast, errorMessage));
                    }
                }
            }
        }

        #endregion

        #region Rule evaluation

        protected override void OnValidate(PropertyInfo property, object propertyValue)
        {
            // Note that propertyValue is never used here. The fluent validation engine will 
            // bind directly to view model properties in order to get their values.

            var propertyName = property.Name;

            // if there is no rules attached to the property then just ignore it.
            if (_compiledRules.ContainsKey(propertyName) == false) return;

            // retrieve all evaluable rules
            var allPropertyRules = (from cr in _compiledRules[propertyName] select cr.Evaluate()).ToList();

            // process evaluation of the rules
            allPropertyRules.ForEach(t => t.RunSynchronously());
            
            Task.Factory.ContinueWhenAll(allPropertyRules.ToArray() 
                , terminatedTasks =>
                      {
                          var evaluationResults = from ttask in terminatedTasks select ttask.Result;

                          // update error messages
                          UpdateCurrentPropertyErrors(propertyName, evaluationResults);
                          UpdateConcernedPropertyErrors(propertyName, evaluationResults);

                          // notify ui to refresh errors
                          NotifyUIErrors(propertyName, evaluationResults);
                      }
                , new CancellationToken(false)
                , TaskContinuationOptions.ExecuteSynchronously
                , TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateCurrentPropertyErrors(string propertyName, IEnumerable<EvaluationResult> evaluationResults)
        {
            var errorCollection = Errors.GetOrAdd(propertyName, new ErrorCollection());
            errorCollection.Clear();

            foreach (var evaluationResult in evaluationResults.Where(er => er.AstResult == false).ToList())
            {
                errorCollection.AddError(evaluationResult.RuleId, evaluationResult.ErrorMessage);
            }
        }

        private void UpdateConcernedPropertyErrors(string propertyName, IEnumerable<EvaluationResult> evaluationResults)
        {
            foreach (var evaluationResult in evaluationResults)
            {
                // removes validation error for shared rule from concerned property
                // validation error should appear on the current property 
                var concernedPropertyNames = evaluationResult.ConcernedProperties
                                                    .Where(p => p.PropertyMetadata.Name != propertyName)
                                                    .Select(p => p.PropertyMetadata.Name).ToList();
                foreach (var concernedPropertyName in concernedPropertyNames)
                {
                    // for each concerned property, get its error collection and remove the current rule id
                    var errorCollection = Errors.GetOrAdd(concernedPropertyName, new ErrorCollection());
                    errorCollection.RemoveError(evaluationResult.RuleId);
                }
            }
        }

        private void NotifyUIErrors(string propertyName, IEnumerable<EvaluationResult> evaluationResults)
        {
            var concernedProperties = (from eResult in evaluationResults
                                       from cProperty in eResult.ConcernedProperties
                                       where cProperty.PropertyMetadata.Name != propertyName
                                       select cProperty.PropertyMetadata.Name).ToArray();

            OnRaiseErrorsChangedEvent(propertyName, concernedProperties);
        }

        #region Private classes

        private class CompiledRule
        {
            private readonly string _ruleId;
            private readonly HashSet<IViewModelProperty> _concernedProperties;
            private readonly ExpressionNode _ast;
            private readonly string _errorMessage;

            public CompiledRule(string ruleId, HashSet<IViewModelProperty> concernedProperties, ExpressionNode ast, string errorMessage)
            {
                _ruleId = ruleId;
                _concernedProperties = concernedProperties;
                _ast = ast;
                _errorMessage = errorMessage;
            }

            public Task<EvaluationResult> Evaluate()
            {
                // wrap the AST task so that instead of returning only one boolean, it returns also the error message
                return new Task<EvaluationResult>(() =>
                                                      {
                                                          var astTask = _ast.Evaluate();
                                                          astTask.RunSynchronously();
                                                          return new EvaluationResult { 
                                                                      RuleId = _ruleId,
                                                                      ConcernedProperties = _concernedProperties,
                                                                      AstResult = astTask.Result, 
                                                                      ErrorMessage = _errorMessage};
                                                      });
            }
        }

        private class EvaluationResult
        {
            public string RuleId { get; set; }
            public bool AstResult { get; set; }
            public string ErrorMessage { get; set; }
            public HashSet<IViewModelProperty> ConcernedProperties { get; set; }
        }

        #endregion
    }

    #endregion

/// <summary>
    /// Thrown when the definition of a rule contains syntactic error or is not complete.
    /// </summary>
    public class RuleDefinitionException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleDefinitionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RuleDefinitionException(string message)
            : base(message)
        { }
    }
}
