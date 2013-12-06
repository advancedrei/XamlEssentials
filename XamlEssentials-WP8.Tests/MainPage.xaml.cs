using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using XamlEssentials.Storage;
using XamlEssentials.WP8.Tests.Resources;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.Core;
using vstest_executionengine_platformbridge;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using System.Reflection;

namespace XamlEssentials.WP8.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {


        public static readonly StoredItem<List<long>> ReadItemIds = new StoredItem<List<long>>("readItemIds", new List<long> { 0 });

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //AsynchronizationContext.Register();
            var wrapper = new TestExecutorServiceWrapper();
            new Thread(new ServiceMain((param0, param1) => wrapper.SendMessage((ContractName)param0, param1)).Run).Start();

        }



    }
}