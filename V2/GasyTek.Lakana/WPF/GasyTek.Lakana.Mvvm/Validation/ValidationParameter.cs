using System.Reflection;
using System.Threading;
using GasyTek.Lakana.Mvvm.ViewModelProperties;

namespace GasyTek.Lakana.Mvvm.Validation
{
    /// <summary>
    /// Serves as parameter for validation method of a validation engine.
    /// </summary>
    public struct ValidationParameter
    {
        private readonly PropertyInfo _propertyMetadata;
        private readonly IViewModelProperty _propertyInstance;
        private readonly object _propertyValue;
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationParameter"/> struct.
        /// </summary>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <param name="propertyInstance">The property instance.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public ValidationParameter(PropertyInfo propertyMetadata, IViewModelProperty propertyInstance, object propertyValue, CancellationToken cancellationToken) : this()
        {
            _propertyMetadata = propertyMetadata;
            _propertyInstance = propertyInstance;
            _propertyValue = propertyValue;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Gets the property metadata.
        /// </summary>
        public PropertyInfo PropertyMetadata
        {
            get { return _propertyMetadata; }
        }

        /// <summary>
        /// Gets the property instance.
        /// </summary>
        public IViewModelProperty PropertyInstance
        {
            get { return _propertyInstance; }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public object PropertyValue
        {
            get { return _propertyValue; }
        }

        /// <summary>
        /// Gets the cancellation token.
        /// </summary>
        public CancellationToken CancellationToken
        {
            get { return _cancellationToken; }
        }
    }
}