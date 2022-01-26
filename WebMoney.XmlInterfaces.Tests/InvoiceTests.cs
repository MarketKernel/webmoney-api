using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class InvoiceTests
    {
        static InvoiceTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateInvoice_PayInvoice_ReturnsRecentTransfer(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();
            
            // X1. Выписка счета.
            var orderId = requestConfiguration.GetUniqueId();
            var originalInvoice = new OriginalInvoice(orderId, requestConfiguration.GetPrimaryWmId(),
                requestConfiguration.GetSecondaryPurse(), (Amount) 0.01M);
            originalInvoice.Description = (Description)("Проверка оплаты счета");
            originalInvoice.Address = (Description)"Адрес для тестов";
            originalInvoice.Expiration = 4;

            var recentInvoice = originalInvoice.Submit();

            Assert.IsNotNull(recentInvoice);

            // X2. Оплата счета.

            var tranId = requestConfiguration.GetUniqueId();

            var originalTransfer = new OriginalTransfer(tranId, requestConfiguration.GetPrimaryPurse(),
                recentInvoice.Invoice.TargetPurse, recentInvoice.Invoice.Amount)
            {
                Description = (Description)"Тест оплаты по счету",
                Force = true
            };

            var recentTransfer = originalTransfer.Submit();

            Assert.IsNotNull(recentTransfer);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateInvoice_ObtainOutgoingInvoice_ReturnsOutgoingInvoice(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // X1. Выписка счета.
            var orderId = requestConfiguration.GetUniqueId();
            var originalInvoice = new OriginalInvoice(orderId, requestConfiguration.GetPrimaryWmId(),
                requestConfiguration.GetSecondaryPurse(), (Amount) 0.01M)
            {
                Description = (Description) ("Проверка оплаты счета"),
                Address = (Description) "Адрес для тестов",
                Expiration = 4
            };

            var recentInvoice = originalInvoice.Submit();

            Assert.IsNotNull(recentInvoice);

            // X4. История выписанных счетов (поиск по номеру счета)
            var outgoingInvoiceFilter = new OutgoingInvoiceFilter(requestConfiguration.GetSecondaryPurse(),
                DateTime.Now.AddMinutes(-5), DateTime.Now.AddMinutes(1))
            {
                InvoiceId = recentInvoice.Invoice.PrimaryId
            };
            var outgoingInvoiceRegister = outgoingInvoiceFilter.Submit();

            Assert.AreEqual(1, outgoingInvoiceRegister.OutgoingInvoiceList.Count);

            var outgoingInvoice = outgoingInvoiceRegister.OutgoingInvoiceList[0];

            Assert.AreEqual(recentInvoice.Invoice.Amount, outgoingInvoice.Amount);

            if (AuthorizationMode.Classic == authorizationMode)
            {
                // Отменяем счет (недокументированный интерфейс)
                var invoiceRefusal = new InvoiceRefusal(outgoingInvoice.SourceWmId, recentInvoice.Invoice.PrimaryId);
                var invoiceReport = invoiceRefusal.Submit();

                Assert.AreEqual(InvoiceState.Refusal, invoiceReport.State);
            }
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateInvoice_ObtainIncomingInvoice_ReturnsIncomingInvoice(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // X1. Выписка счета.
            var orderId = requestConfiguration.GetUniqueId();
            var originalInvoice = new OriginalInvoice(orderId, requestConfiguration.GetPrimaryWmId(),
                requestConfiguration.GetSecondaryPurse(), (Amount)0.01M)
            {
                Description = (Description)("Проверка оплаты счета"),
                Address = (Description)"Адрес для тестов",
                Expiration = 4
            };

            var recentInvoice = originalInvoice.Submit();

            Assert.IsNotNull(recentInvoice);

            // X10. Список счетов на оплату
            var incomingInvoiceFilter = new IncomingInvoiceFilter(requestConfiguration.GetPrimaryWmId(),
                DateTime.Now.AddDays(-1), DateTime.Now.AddMinutes(1));
            var incomingInvoiceRegister = incomingInvoiceFilter.Submit();

            Assert.AreNotEqual(0, incomingInvoiceRegister.IncomingInvoiceList.Count);

            if (authorizationMode == AuthorizationMode.Classic)
            {
                foreach (var incomingInvoice in incomingInvoiceRegister.IncomingInvoiceList)
                {
                    if (InvoiceState.NotPaid == incomingInvoice.InvoiceState)
                    {
                        // Отменяем счет
                        var invoiceRefusal = new InvoiceRefusal(incomingInvoice.TargetWmId, incomingInvoice.PrimaryId);
                        var invoiceReport = invoiceRefusal.Submit();

                        Assert.IsNotNull(invoiceReport);
                    }
                }
            }
        }
    }
}