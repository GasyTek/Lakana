using System;
using System.Windows.Markup;
using System.Windows;
using System.Reflection;
using GasyTek.Lakana.Common.Communication;

namespace GasyTek.Lakana.Common.UI
{
    /// <summary>
    /// Markup extension that support dynamic localization of texts.
    /// The RESX resource file have to be publicly accessible.
    /// </summary>
    public class TextResourceExtension : MarkupExtension
    {
        #region Fields

        private string _member;
        private Type _memberType;
        private object _targetObject;
        private object _targetProperty;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        [ConstructorArgument("member")]
        public string Member
        {
            get { return _member; }
            set
            {
                if (value == null) { throw new ArgumentNullException("value"); }
                _member = value;
            }
        }

        internal Type MemberType
        {
            get { return _memberType; }
            set
            {
                if (value == null) { throw new ArgumentNullException("value"); }
                _memberType = value;
            }
        }

        internal string FieldName { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TextResourceExtension"/> class.
        /// </summary>
        public TextResourceExtension()
        {
            SubscribeToCultureChangedEvent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextResourceExtension"/> class.
        /// </summary>
        public TextResourceExtension(string member)
            : this()
        {
            if (member == null) { throw new ArgumentNullException("member"); }
            _member = member;
        }

        #endregion

        #region Overriden methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Member == null) { throw new InvalidOperationException("The Member property must be non null."); }

            // Save the target object and the target property
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (service != null)
            {
                _targetObject = service.TargetObject;
                _targetProperty = service.TargetProperty;
            }

            // Initialize membertype member
            if (MemberType == null)
            {
                var index = Member.IndexOf('.');
                if (index < 0)
                {
                    throw new ArgumentException(
                        String.Format(
                            "'{0}' TextResourceExtension value cannot be resolved to an enumeration, static field, or static property.",
                            Member));
                }

                var qualifiedTypeName = Member.Substring(0, index);
                if (qualifiedTypeName == string.Empty)
                {
                    throw new ArgumentException(
                        String.Format(
                            "'{0}' TextResourceExtension value cannot be resolved to an enumeration, static field, or static property.",
                            Member));
                }

                var xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
                if (xamlTypeResolver == null)
                {
                    throw new ArgumentException(
                        String.Format(
                            "Markup extension '{0}' requires '{1}' be implemented in the IServiceProvider for ProvideValue.",
                            GetType().Name, "IXamlTypeResolver"));
                }

                MemberType = xamlTypeResolver.Resolve(qualifiedTypeName);
                FieldName = Member.Substring(index + 1, (Member.Length - index) - 1);
                if (String.IsNullOrEmpty(FieldName))
                {
                    throw new ArgumentException(
                         String.Format(
                             "'{0}' TextResourceExtension value cannot be resolved to an enumeration, static field, or static property.",
                             Member));
                }
            }

            return GetCurrentValue();
        }

        #endregion

        #region Private methods

        private void SubscribeToCultureChangedEvent()
        {
            MessageBus.Subscribe<CultureSettingsChangedEvent>(c => UpdateValue(GetCurrentValue()));
        }

        private object GetCurrentValue()
        {
            // if the static member is an enum
            if (MemberType.IsEnum) { return Enum.Parse(MemberType, FieldName); }

            // if the static member is a field
            var fieldInfo = MemberType.GetField(FieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(null);
            }

            // if the static member is a property
            var propertyInfo = MemberType.GetProperty(FieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(null, null);
            }

            throw new ArgumentException(
                String.Format(
                    "'{0}' TextResourceExtension value cannot be resolved to an enumeration, static field, or static property.",
                    MemberType.FullName + "." + Member));
        }

        private void UpdateValue(object value)
        {
            if (_targetObject == null) return;

            if (_targetProperty is DependencyProperty)
            {
                var obj = _targetObject as DependencyObject;
                var prop = _targetProperty as DependencyProperty;

                if (obj != null)
                {
                    Action updateAction = () => obj.SetValue(prop, value);

                    // Check whether the target object can be accessed from the
                    // current thread, and use Dispatcher.Invoke if it can't
                    if (obj.CheckAccess()) updateAction();
                    else obj.Dispatcher.Invoke(updateAction);
                }
            }
            else
            {
                // _targetProperty is PropertyInfo
                var prop = _targetProperty as PropertyInfo;
                if (prop != null) prop.SetValue(_targetObject, value, null);
            }
        }

        #endregion
    }
}
