using System;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GasyTek.Lakana.Common.Utils
{
    /// <summary>
    /// Helper class to accessing Resources.
    /// </summary>
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
    }
}
