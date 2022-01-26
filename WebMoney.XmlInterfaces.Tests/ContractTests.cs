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
    public sealed class ContractTests
    {
        static ContractTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        public void CreateContract_FilterAcceptors_ReturnsAcceptorRegister(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var originalContract = new OriginalContract((Description) "Тестовый контракт",
                "Контракт создан в целях тестирования библиотеки WM-API.");
            
            originalContract.AcceptorList.Add(requestConfiguration.GetPrimaryWmId());
            originalContract.AcceptorList.Add(requestConfiguration.GetSecondaryWmId());

            var recentContract = originalContract.Submit();

            Assert.IsNotNull(recentContract);

            var acceptorFilter = new AcceptorFilter(recentContract.ContractId);
            var acceptorRegister = acceptorFilter.Submit();

            Assert.AreNotEqual(0, acceptorRegister.AcceptorList.Count);
        }
    }
}
