using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeFluentValidationEngine : FluentValidationEngine<FakeEditableViewModel>
    {
        public Action DefineRulesAction { get; set; }

        /// <summary>
        /// Used by the unit testing framework to wait for a result of 
        /// an asynchronous operation before testing the result.
        /// </summary>
        public Task SynchronizationTask { get; private set; }

        public FakeFluentValidationEngine(FakeEditableViewModel viewModelInstance) 
            : base(viewModelInstance, false)
        {
            SynchronizationTask = new Task(DummyAction);
        }

        private void DummyAction() { }

        protected override void DefineRules()
        {
            if (DefineRulesAction != null)
                DefineRulesAction();
        }

        public IFluentVerb<FakeEditableViewModel> RequiresThat2(Expression<Func<FakeEditableViewModel, IViewModelProperty>> propertyExpression)
        {
            return RequiresThat(propertyExpression);
        }

        protected override void OnRaiseErrorsChangedEvent(string propertyName)
        {
            // surrounded by try/catch to avoid the error that appears
            // when trying to start a task that alread started.
            try { SynchronizationTask.Start(); }
            catch {}
        }
    }
}
