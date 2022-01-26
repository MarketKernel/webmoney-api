using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public class WmIdFinderTests
    {
        static WmIdFinderTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FindByWmId_ReturnsWmIdReport(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var wmIdFinder = new WmIdFinder(requestConfiguration.GetSecondaryWmId());
            var wmIdReport = wmIdFinder.Submit();

            Assert.AreEqual(PassportDegree.Developer, wmIdReport.Passport);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FindByPurse_ReturnsWmIdReport(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var wmIdFinder = new WmIdFinder(requestConfiguration.GetPrimaryPurse());
            var wmIdReport = wmIdFinder.Submit();

            Assert.AreEqual(PassportDegree.Developer, wmIdReport.Passport);
        }
    }
}
