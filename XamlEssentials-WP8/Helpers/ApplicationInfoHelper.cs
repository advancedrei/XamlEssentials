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
    /// Helper functions for retrieving the data from Microsoft's application manifest formats.
    /// </summary>
    public static class ApplicationInfoHelper
    {

        #region Private Members

        const string AppManifestName = "WMAppManifest.xml";
        const string AppNodeName = "App";

        #endregion

        #region Properties

        /// <summary>
        /// The title of the app that appears in the app list or Games Hub.
        /// </summary>
        public static string Title { get; private set; }

        /// <summary>
        /// The version of the app.
        /// </summary>
        public static string Version { get; private set; }

        /// <summary>
        /// The app author’s name.
        /// </summary>
        public static string Author { get; private set; }

        /// <summary>
        /// The description of the app.
        /// </summary>
        public static string Description { get; private set; }

        /// <summary>
        /// The Guid for the product. NOTE: This ID is inserted into the manifest after it is submitted to the Store.
        /// </summary>
        public static Guid ProductId { get; private set; }

        /// <summary>
        /// The Guid for the Publisher. NOTE: This ID is inserted into the manifest after it is submitted to the Store.
        /// </summary>
        public static Guid PublisherId { get; private set; }

        /// <summary>
        /// Indicates whether the app is a beta app. This has consequences for the app license.
        /// </summary>
        public static bool IsBeta { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// The static constructor for the ApplicationInfoHelper.
        /// </summary>
        /// <remarks>
        /// Because this information does not change at runtime, this is the most efficient method of retrieving the data from the manifest.
        /// This is because we only grab the information once, when the helper is first accessed.
        /// </remarks>
        static ApplicationInfoHelper()
        {
            Title = string.Empty;
            Version = string.Empty;
            Author = string.Empty;
            Description = string.Empty;
            ProductId = Guid.Empty;
            PublisherId = Guid.Empty;
            IsBeta = false;

            try
            {
#if WINDOWS_PHONE
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
                    ProductId = Guid.Parse(rdr.GetAttribute("ProductID"));
                    PublisherId = Guid.Parse(rdr.GetAttribute("PublisherID"));
                    IsBeta = bool.Parse(rdr.GetAttribute("IsBeta"));
                }
#elif WINRT
                var package = Package.Current;
                var version = package.Id.Version;
                Version = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
                PublisherId = Guid.Parse(package.Id.PublisherId);
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
                ProductId = Guid.Parse(doc.Descendants(xname + "Identity").First().Attribute("Name").Value);
#endif

#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred with the ApplicationInfoHelper: " + ex.Message);
            }

        }

        #endregion

    }

}
