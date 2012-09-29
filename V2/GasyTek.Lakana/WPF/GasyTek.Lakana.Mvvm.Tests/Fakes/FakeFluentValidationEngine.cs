using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeFluentValidationEngine : FluentValidationEngine<FakeEditableViewModel>
    {
        public event EventHandler ValidationTerminated;

        public Action DefineRulesAction { get; set; }

        public FakeFluentValidationEngine(FakeEditableViewModel viewModelInstance) 
            : base(viewModelInstance, false)
        {
        }

        protected override void OnDefineRules()
        {
            if (DefineRulesAction != null)
                DefineRulesAction();
        }

        protected internal override void StartValidationTasks(List<Task<EvaluationResult>> validationTasks)
        {
            validationTasks.ForEach(t => t.RunSynchronously());
        }

        protected override void OnValidationTerminated(string propertyName, List<Task<EvaluationResult>> terminatedTasks)
        {
            base.OnValidationTerminated(propertyName, terminatedTasks);

            // notify the termination of the tasks
            var handler = ValidationTerminated;
            if (handler != null) handler(this, new EventArgs());
        }

        public IFluentVerb<FakeEditableViewModel, TPropertyValue> AssertThatProperty<TPropertyValue>(Expression<Func<FakeEditableViewModel, IViewModelProperty>> propertyExpression)
        {
            return Property<TPropertyValue>(propertyExpression);
        }
    }
}
