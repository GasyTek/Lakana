using System;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using GasyTek.Lakana.Common.Attributes;

namespace GasyTek.Lakana.Common.Utils
{
    public class ResourceUtil
    {
        /// <summary>
        /// Obtain the text resource corresponding to this resource ID from the provided resource class.
        /// </summary>
        /// <param name="resourceId">The identifier of the resource.</param>
        /// <param name="resourceType">The type of the object that will provide the resource.</param>
        /// <returns></returns>
        public static string GetResource(string resourceId, Type resourceType)
        {
            var resourceValue = String.Empty;
            var resourceManager =
                    resourceType.InvokeMember(
                    @"ResourceManager",
                    BindingFlags.GetProperty | BindingFlags.Static |
                    BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    null,
                    new object[] { }) as ResourceManager;

            var culture =
                 resourceType.InvokeMember(
                 @"Culture",
                 BindingFlags.GetProperty | BindingFlags.Static |
                 BindingFlags.Public | BindingFlags.NonPublic,
                 null,
                 null,
                 new object[] { }) as CultureInfo;

            if (resourceManager != null)
            {
                resourceValue = resourceManager.GetString(resourceId, culture);
            }
            return resourceValue;
        }

        /// <summary>
        /// Gets an accessor to the resource specified
        /// </summary>
        /// <param name="resourceId">The identifier of the resource.</param>
        /// <param name="resourceType">The type of the object that will provide the resource.</param>
        public static Func<string> GetResourceAccessor(string resourceId, Type resourceType)
        {
            return () => GetResource (resourceId, resourceType);
        }

        /// <summary>
        /// Get the resource ID defined for the enum member.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumMemberName">The name of the member requested.</param>
        /// <returns>The identifier that is associated with the enum member specified</returns>
        /// <remarks>The resource id can be specified by using <see cref="LocalizationEnumAttribute"/></remarks>
        public static string GetResourceId(Type enumType, string enumMemberName)
        {
            var resourceId = String.Empty;
            var fieldInfo = enumType.GetField(enumMemberName);
            if (fieldInfo != null)
            {
                var localizeAttributes = fieldInfo.GetCustomAttributes(typeof(LocalizationEnumAttribute), false) as LocalizationEnumAttribute[];
                if (localizeAttributes != null && localizeAttributes.Count() == 1)
                {
                    resourceId = localizeAttributes[0].LocalizationID;
                }
            }
            return resourceId;
        }

        /// <summary>
        /// Retrieve an image source from an absolute Uri.
        /// </summary>
        /// <param name="imageUri">An absolute uri</param>
        public static ImageSource GetImageSourceFrom(Uri imageUri)
        {
            var imageSource = new BitmapImage();
            imageSource.BeginInit();
            imageSource.UriSource = imageUri;
            imageSource.EndInit();
            return imageSource;
        }

        /// <summary>
        /// Retrieve an image source that points to the absolute Uri.
        /// </summary>
        public static ImageSource GetImageSourceFromAbsolute(string imageAbsoluteUri)
        {
            if (! Uri.IsWellFormedUriString(imageAbsoluteUri, UriKind.Absolute)) return null;
            var uri = new Uri(imageAbsoluteUri);
            return GetImageSourceFrom (uri);
        }

        /// <summary>
        /// Retrieve an image source that points to the relative Uri (relative to the caller assembly)
        /// </summary>
        /// <param name="imageRelativeUri">A relative uri according to assembly GasyTek.Tantana.Common</param>
        public static ImageSource GetImageSourceFromRelative(string imageRelativeUri)
        {
            //pack://application:,,,/ReferencedAssembly;component/Subfolder/ResourceFile.xaml

            var absoluteUri = new StringBuilder();
            absoluteUri.Append ("pack://application:,,,");
            absoluteUri.Append("/GasyTek.Tantana.Infrastructure.Presentation;");
            absoluteUri.Append("component" + Path.Combine ("/", imageRelativeUri));
            return GetImageSourceFromAbsolute(absoluteUri.ToString());
        }

    }
}
