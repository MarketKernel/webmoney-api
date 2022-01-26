using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class PurseTests
    {
        static PurseTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreatePurse_ReturnsPurseNumber(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var originalPurse = new OriginalPurse(requestConfiguration.GetPrimaryWmId(), WmCurrency.Z,
                (Description) "Test Z");
            var recentPurse = originalPurse.Submit();

            Assert.IsNotNull(recentPurse);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FilterPurses_ReturnsPurseInfoRegister(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var purseInfoFilter = new PurseInfoFilter(requestConfiguration.GetPrimaryWmId());
            var purseInfoRegister = purseInfoFilter.Submit();

            Assert.AreNotEqual(0, purseInfoRegister.PurseInfoList.Count);
        }
    }
}
