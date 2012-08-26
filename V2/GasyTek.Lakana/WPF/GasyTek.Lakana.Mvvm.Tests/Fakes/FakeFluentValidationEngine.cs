using System;
using System.Linq.Expressions;
using GasyTek.Lakana.Mvvm.Validation.Fluent;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    class FakeFluentValidationEngine : FluentValidationEngine<FakeEditableViewModel>
    {
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

        public IFluentVerb<FakeEditableViewModel> TestProperty(Expression<Func<FakeEditableViewModel, IViewModelProperty>> propertyExpression)
        {
            return Property(propertyExpression);
        }
    }
}
