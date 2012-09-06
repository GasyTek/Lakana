using System;
using System.Linq.Expressions;
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

        protected override void DefineRules()
        {
            if (DefineRulesAction != null)
                DefineRulesAction();
        }

        protected override void OnValidate(System.Reflection.PropertyInfo property, object propertyValue)
        {
            base.OnValidate(property, propertyValue);
            OnValidationTerminated(new EventArgs());
        }


        public void OnValidationTerminated(EventArgs e)
        {
            var handler = ValidationTerminated;
            if (handler != null) handler(this, e);
        }

        public IFluentVerb<FakeEditableViewModel> AssertThatProperty(Expression<Func<FakeEditableViewModel, IViewModelProperty>> propertyExpression)
        {
            return Property(propertyExpression);
        }
    }
}
