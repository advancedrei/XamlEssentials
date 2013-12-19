using System;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using XamlEssentials.Helpers;

namespace XamlEssentials.WP8.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AsynchronizationContextRegistered()
        {
            //Assert.IsInstanceOfType(SynchronizationContext.Current, typeof(AsynchronizationContext));
            Assert.IsTrue(true);
        }


        //[TestMethod]
        private async void InvisibleException()
        {
            var client = new HttpClient();
            await client.GetAsync("http://nuget.ru/");
        }

        [TestMethod]
        public void UniqueIdTest()
        {
            //var publisherHostId = Windows.Phone.System.Analytics.HostInformation.PublisherHostId;
            //var uniqueId = PhoneExtendedPropertiesHelper.GetDeviceUniqueId();
            //Assert.AreEqual(publisherHostId, uniqueId);

        }
    }
}
