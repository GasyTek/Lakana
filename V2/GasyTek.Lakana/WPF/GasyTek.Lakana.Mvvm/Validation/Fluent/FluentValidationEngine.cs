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
        private readonly Dictionary<string, List<FluentImplementer<TViewModel>>> _definedRules;

        // contains the couple (AST, Error message) for each property.
        private readonly Dictionary<string, List<CompiledRule>> _compiledRules;

        /// <summary>
        /// Do not use this constructor, it is intended to be used by the infrastructure only.
        /// </summary>
        internal FluentValidationEngine(TViewModel viewModelInstance, bool buildRuleImmediatly)
        {
            _parser = new Parser();
            _definedRules = new Dictionary<string, List<FluentImplementer<TViewModel>>>();
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
        /// </summary>
        protected abstract void DefineRules();

        /// <summary>
        /// Allows to define rule for one property.
        /// </summary>
        /// <param name="propertyExpression">The expression that specify the property to define rule for.</param>
        /// <remarks>This method will typically called from <see cref="DefineRules"/> method.</remarks>
        protected IFluentVerb<TViewModel> Property(
            Expression<Func<TViewModel, IViewModelProperty>> propertyExpression)
        {
            var property = propertyExpression.Compile()(_viewModelInstance);
            if (property == null)
                throw new InvalidOperationException(
                    "Property instance is null. Please verify that you assigned a model to your view model.");

            var propertyName = property.PropertyMetadata.Name;
            var fluentApi = new FluentImplementer<TViewModel>(_viewModelInstance);

            // registers the rule for this property 
            if (!_definedRules.ContainsKey(propertyName))
                _definedRules.Add(propertyName, new List<FluentImplementer<TViewModel>>());

            _definedRules[propertyName].Add(fluentApi);

            return fluentApi.Property(propertyExpression);
        }

        private void EnsureRulesAreValid()
        {
            var allDefinedRules = from definedRule in _definedRules
                                  from expr in definedRule.Value
                                  select expr;

            allDefinedRules.ToList().ForEach(rule =>
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
            // for each property that have a defined rule 
            foreach (var definedRule in _definedRules)
            {
                // for each rule define on the property
                foreach (var expression in definedRule.Value)
                {
                    // compile the rule to an abstract syntax tree
                    var ast = _parser.Parse(expression.InternalTokens);
                    var concernedProperties = expression.Context.Properties;
                    var errorMessage = expression.Context.Message;

                    // attach the compiled rule to all concerned properties
                    foreach (var property in concernedProperties)
                    {
                        var propertyName = property.PropertyMetadata.Name;

                        if (_compiledRules.ContainsKey(propertyName) == false)
                            _compiledRules.Add(propertyName, new List<CompiledRule>());

                        _compiledRules[propertyName].Add(new CompiledRule (ast, errorMessage));
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

            // retrieve all evaluation tasks
            var allEvaluationTasks = (from cr in _compiledRules[propertyName] select cr.Evaluate()).ToList();

            // starts all evaluation tasks
            allEvaluationTasks.ForEach(t => t.Start());

            Task.Factory.ContinueWhenAll(allEvaluationTasks.ToArray() 
                , terminatedTasks =>
                      {
                          var propertyErrors = from Task<EvaluationResult> ttask in terminatedTasks
                                               where ttask.Result.AstResult == false
                                               select ttask.Result.ErrorMessage;

                          if (propertyErrors.Any())
                          {
                              // adds all detected errors for this property
                              var localPropertyErrors = propertyErrors.ToList();
                              Errors.AddOrUpdate(propertyName, localPropertyErrors, (key, currentValue) => localPropertyErrors);
                          }
                          else
                          {
                              // reset errors for this property
                              List<string> localPropertyErrors;
                              Errors.TryRemove(propertyName, out localPropertyErrors);
                          }

                          OnRaiseErrorsChangedEvent(propertyName);
                      }
                , new CancellationToken(false)
                , TaskContinuationOptions.None
                , TaskScheduler.FromCurrentSynchronizationContext());
        }

        private class CompiledRule
        {
            private readonly ExpressionNode _ast;
            private readonly string _errorMessage;

            public CompiledRule(ExpressionNode ast, string errorMessage)
            {
                _ast = ast;
                _errorMessage = errorMessage;
            }

            public Task<EvaluationResult> Evaluate()
            {
                // wrap the ast task so that instead of returning only one boolean, it returns also the error message
                return new Task<EvaluationResult>(() =>
                                                      {
                                                          var astTask = _ast.Evaluate();
                                                          astTask.Start();
                                                          astTask.Wait();
                                                          return new EvaluationResult { AstResult = astTask.Result, ErrorMessage = _errorMessage};
                                                      });
            }
        }

        private class EvaluationResult
        {
            public bool AstResult { get; set; }
            public string ErrorMessage { get; set; }
        }
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
