using System.Windows;
using System.Windows.Data;

namespace System.Windows
{
    /// <summary>
    /// Extension methods for the FrameworkElement object.
    /// </summary>
    public static class FrameworkElementExtensions
    {

        /// <summary>
        /// Allows you to manually trigger an update to a FrameworkElement's DataBindings for a given DependencyProperty.
        /// </summary>
        /// <param name="frameworkElement">The FrameworkElement (XAML object) with the source of the update.</param>
        /// <param name="dependencyProperty">The DependencyProperty you would like to set based on the binding from the FrameworkElement.</param>
        /// <remarks>Method courtesy of http://sharpsnippets.wordpress.com/2013/12/01/updatesource-extension-method. </remarks>
        public static void UpdateSource(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            BindingExpression expression = frameworkElement.GetBindingExpression(dependencyProperty);
            if (expression != null)
            {
                expression.UpdateSource();
            }
        }
    }
}
