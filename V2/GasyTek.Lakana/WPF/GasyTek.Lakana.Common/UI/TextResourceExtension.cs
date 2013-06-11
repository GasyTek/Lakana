using System;
using System.Windows.Markup;
using System.Reflection;
using GasyTek.Lakana.Common.Communication;
using GasyTek.Lakana.Common.Base;
using GasyTek.Lakana.Common.Extensions;
using System.Windows.Data;

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

            var binding = new Binding("Value") { Source = new TextResourceValue(MemberType, Member, FieldName) };
            return binding.ProvideValue(serviceProvider);
        }

        #endregion


        #region Private class TextResourceValue

        private class TextResourceValue : NotifyPropertyChangedBase, IMessageListener<CultureSettingsChangedEvent>
        {
            private object _value;
            private readonly Type _memberType;
            private readonly string _member;
            private readonly string _fieldName;

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value
            {
                get { return _value; }
                set { this.SetPropertyValueAndNotify(ref _value, value, trv => trv.Value); }
            }

            public TextResourceValue(Type memberType, string member, string fieldName)
            {
                _memberType = memberType;
                _member = member;
                _fieldName = fieldName;
                _value = GetCurrentValue(_memberType, _member, _fieldName);
                SubscribeToCultureChangedEvent();
            }

            private void SubscribeToCultureChangedEvent()
            {
                MessageBus.Subscribe(this);
            }

            #region IMessageListener members

            public ISubscriptionToken<CultureSettingsChangedEvent> SubscriptionToken { get; set; }

            public void OnMessageReceived(CultureSettingsChangedEvent message)
            {
                Value = GetCurrentValue(_memberType, _member, _fieldName);
            }

            #endregion

            private object GetCurrentValue(Type memberType, string member, string fieldName)
            {
                // if the static member is an enum
                if (memberType.IsEnum)
                {
                    return Enum.Parse(memberType, fieldName);
                }

                // if the static member is a field
                var fieldInfo = memberType.GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(null);
                }

                // if the static member is a property
                var propertyInfo = memberType.GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(null, null);
                }

                throw new ArgumentException(
                    String.Format(
                        "'{0}' TextResourceExtension value cannot be resolved to an enumeration, static field, or static property.",
                        memberType.FullName + "." + member));
            }


        }

        #endregion
    }
}
