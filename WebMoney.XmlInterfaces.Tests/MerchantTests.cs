using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class MerchantTests
    {
        static MerchantTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Merchant)]
        public void CreateMerchantPayment_ReturnsPaymentToken(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var originalMerchantPayment = new OriginalMerchantPayment(
                requestConfiguration.GetUniqueId(), requestConfiguration.GetSecondaryPurse(), (Amount) 0.01,
                (Description) "Тестовый платеж (проверка API)", 1);

            var merchantPaymentToken = originalMerchantPayment.Submit();

            Assert.IsNotNull(merchantPaymentToken.Token);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Merchant)]
        public void FindMerchantOperation_ReturnsMerchantOperation(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var operationObtainer = new MerchantOperationObtainer(requestConfiguration.GetSecondaryPurse(), 1848219521, PaymentNumberKind.TransferPrimaryId);

            var merchantOperation = operationObtainer.Submit();
            
            Assert.IsNotNull(merchantOperation);
        }
    }
}
