using System;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;

namespace XamlEssentials
{
    /// <summary>
    /// Attribute for localization.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class LocalizableDescriptionAttribute : DescriptionAttribute
    {

        #region Private Variables

        private readonly Type _resourcesType;
        private bool _isLocalized;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="resourcesType">Type of the resources.</param>
        public LocalizableDescriptionAttribute(string description, Type resourcesType)
            : base(description)
        {
            _resourcesType = resourcesType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the string value from the resources.
        /// </summary>
        /// <value></value>
        /// <returns>The description stored in this attribute.</returns>
        public override string Description
        {
            get
            {
                if (_isLocalized) return DescriptionValue;

                var resMan = _resourcesType.InvokeMember(
                    @"ResourceManager",
                    BindingFlags.GetProperty | BindingFlags.Static |BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    null,
                    new object[] { }) as ResourceManager;

                var culture =  _resourcesType.InvokeMember(
                    @"Culture",
                    BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    null,
                    new object[] { }) as CultureInfo;

                _isLocalized = true;

                if (resMan != null)
                {
                    DescriptionValue = resMan.GetString(DescriptionValue, culture);
                }

                return DescriptionValue;
            }
        }
        #endregion

    }
}
