using System;
using System.Diagnostics;
using System.Xml;

namespace XamlEssentials
{
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
        }



    }
}
