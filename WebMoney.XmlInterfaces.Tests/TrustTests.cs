using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class TrustTests
    {
        static TrustTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FilterOutgoingTrusts_ReturnsTrustRegister(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var outgoingTrustFilter = new OutgoingTrustFilter(requestConfiguration.GetPrimaryWmId());
            var trustRegister = outgoingTrustFilter.Submit();

            Assert.IsNotNull(trustRegister);
            Assert.AreNotEqual(0, trustRegister.TrustList.Count);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FilterIncomingTrusts_ReturnsTrustRegister(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var incomingTrustFilter = new IncomingTrustFilter(requestConfiguration.GetPrimaryWmId());
            var trustRegister = incomingTrustFilter.Submit();

            Assert.IsNotNull(trustRegister);
        }

        //// TODO: не удалось проверить - нельзя отключить подтверждение.
        //[TestMethod]
        //[DataRow(AuthorizationMode.Classic)]
        //[DataRow(AuthorizationMode.Light)]
        //public void CreateTrust_ReturnsRecentTrust(AuthorizationMode authorizationMode)
        //{
        //    var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
        //    requestConfiguration.ApplyInitializer();

        //    var originalTrust = new OriginalTrust(requestConfiguration.GetSecondaryWmId(),
        //        requestConfiguration.GetSecondaryPurse())
        //    {
        //        InvoiceAllowed = true
        //    };

        //    var recentTrust = originalTrust.Submit();
            
        //    Assert.IsNotNull(recentTrust);
        //    Assert.AreEqual(true, recentTrust.InvoiceAllowed);
        //}
    }
}
