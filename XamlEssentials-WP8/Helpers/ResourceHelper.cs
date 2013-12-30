using System;
using System.Windows;

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    public static class ResourceHelper
    {

        /// <summary>
        /// If the device is detected to be a Phablet, the referenced XAML file will be added to the application's MergedDictionaries at runtime.
        /// </summary>
        /// <param name="styleXamlUrl">The absolute Url of the XAML file, in Silverlight Component notation.</param>
        /// <example>
        /// DisplayHelper.AddPhabletStyle("/YourAssemblyName;component/Themes/ItemStyle1080p.xaml");
        /// </example>
        /// <remarks>
        /// Adapted from http://developer.nokia.com/Resources/Library/Lumia/#!optimising-for-nokia-phablets/optimising-layout-for-big-screens.html
        /// </remarks>
        public static void AddPhabletStyle(string styleXamlUrl)
        {
            if (!DisplayHelper.IsPhablet) return;
            AddStyle(styleXamlUrl);
        }

        /// <summary>
        /// Adds the referenced XAML file to the application's MergedDictionaries at runtime.
        /// </summary>
        /// <param name="styleXamlUrl">The absolute Url of the XAML file, in Silverlight Component notation.</param>
        /// <example>
        /// DisplayHelper.AddStyle("/YourAssemblyName;component/Themes/ItemStyle.xaml");
        /// </example>
        public static void AddStyle(string styleXamlUrl)
        {
            var appTheme = new ResourceDictionary
            {
                Source = new Uri(styleXamlUrl, UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Add(appTheme);
        }


    }
}
