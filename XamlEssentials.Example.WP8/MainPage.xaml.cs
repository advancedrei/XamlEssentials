using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using XamlEssentials.Helpers;
using XamlEssentials.Storage;

namespace XamlEssentials.Example.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {

        public static readonly StoredItem<List<long>> ReadItemIds = new StoredItem<List<long>>("readItemIds", new List<long> { 0 });

        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            var test = ReadItemIds.Value;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BetaExperienceHelper.CheckExpiration(DateTime.Now.AddDays(-1));
            //BetaExperienceHelper.CheckExpiration(CalculateFrom.FirstVersion, 90);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}