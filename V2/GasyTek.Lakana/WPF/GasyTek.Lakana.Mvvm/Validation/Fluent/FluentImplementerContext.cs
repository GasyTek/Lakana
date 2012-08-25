using System.Collections.Generic;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Validation.Fluent
{
    /// <summary>
    /// The context object that is used by FluentImplementer to build fluent sentences.
    /// </summary>
    internal class FluentImplementerContext
    {
        /// <summary>
        /// Gets or sets the initial property on which the rule is defined.
        /// </summary>
        public IViewModelProperty OwnerProperty { get; set; }

        /// <summary>
        /// Gets or sets the current property that will be used for next evaluation according to the current context.
        /// </summary>
        public IViewModelProperty CurrentProperty { get; set; }

        /// <summary>
        /// All the properties that are concerned.
        /// </summary>
        public List<IViewModelProperty> Properties { get; private set; }

        /// <summary>
        /// Gets or sets the message that is associated with the fluent rule.
        /// </summary>
        public string Message { get; set; }

        public FluentImplementerContext()
        {
            Properties = new List<IViewModelProperty>();
            Message = "No message defined.";
        }
    }
}