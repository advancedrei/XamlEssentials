using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace XamlEssentials.MarkupExtensions
{

    /// <summary>
    /// A base class used to make awesome Converters with.
    /// </summary>
    /// <typeparam name="T">The type implementing IValueConverter to make awesome.</typeparam>
#if !SILVERLIGHT
    [MarkupExtensionReturnType(typeof(IValueConverter))]
#endif
    public abstract class ConvertibleMarkupExtension<T> : MarkupExtension
        where T : class, IValueConverter, new()
    {

        #region Private Members

        private static T _converter;

        #endregion

        #region MarkupExtension Implementation

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        #endregion

    }
}
