using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class PassportTests
    {
        static PassportTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void FindPassport_ReturnsPassportInfo(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var passportFinder = new PassportFinder(requestConfiguration.GetSecondaryWmId());
            var passport = passportFinder.Submit();

            Assert.IsNotNull(passport);
        }
    }
}
