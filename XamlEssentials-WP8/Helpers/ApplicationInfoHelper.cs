using System;
using System.Diagnostics;
using System.Xml;
#if WINRT
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel;
#endif

namespace XamlEssentials.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationInfoHelper
    {

        #region Private Members

        const string AppManifestName = "WMAppManifest.xml";
        const string AppNodeName = "App";

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static string Title { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static string Version { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static string Author { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static string Description { get; private set; }

        #endregion

        static ApplicationInfoHelper()
        {
            Title = string.Empty;
            Version = string.Empty;
            Author = string.Empty;
            Description = string.Empty;

#if WINDOWS_PHONE
            try
            {
                var settings = new XmlReaderSettings { XmlResolver = new XmlXapResolver() };

                using (var rdr = XmlReader.Create(AppManifestName, settings))
                {
                    rdr.ReadToDescendant(AppNodeName);
                    if (!rdr.IsStartElement())
                    {
                        throw new FormatException(AppManifestName + " is missing " + AppNodeName);
                    }

                    Title = rdr.GetAttribute("Title");
                    Version = rdr.GetAttribute("Version");
                    Author = rdr.GetAttribute("Author");
                    Description = rdr.GetAttribute("Description");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred with the ApplicationInfoHelper: " + ex.Message);
            }
#elif WINRT
            var package = Package.Current;
            var version = package.Id.Version;
            Version = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
#if WINRT81
            Title = package.DisplayName;
            Author = package.PublisherDisplayName;
            Description = package.Description;
#else
            var doc = XDocument.Load("AppxManifest.xml", LoadOptions.None);
            var xname = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");
            var visualElement = doc.Descendants(xname + "VisualElements").First();

            Title = doc.Descendants(xname + "DisplayName").First().Value;
            Author = doc.Descendants(xname + "PublisherDisplayName").First().Value;
            Description = visualElement.Attribute("Description").Value;
#endif
#endif
        }

    }

}
