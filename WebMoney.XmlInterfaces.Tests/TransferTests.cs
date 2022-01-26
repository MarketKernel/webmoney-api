using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class TransferTests
    {
        static TransferTests()
        {
            StaticContextInitializer.Init();
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateTransfer_FindTransfer_ReturnsTransfer(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // X2. Перевод средств.
            var tranId = requestConfiguration.GetUniqueId();
            var originalTransfer = new OriginalTransfer(tranId, requestConfiguration.GetPrimaryPurse(),
                requestConfiguration.GetSecondaryPurse(), (Amount)0.01M)
            {
                Description = (Description)"Тестовая операция",
                Force = true
            };

            var recentTransfer = originalTransfer.Submit();

            // X3. История операций.

            var transferFilter = new TransferFilter(requestConfiguration.GetPrimaryPurse(), DateTime.Now.AddMinutes(-5),
                DateTime.Now.AddMinutes(1));
            var transferRegister = transferFilter.Submit();

            var transfer = transferRegister.TransferList.First(t => t.PrimaryId == recentTransfer.Transfer.PrimaryId);

            Assert.IsNotNull(transfer);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateTransferWithProtection_FinishProtection_ReturnsProtectionReport(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var protectionCode = (Description)"12345";

            // X2. Перевод средств.
            var tranId = requestConfiguration.GetUniqueId();
            var originalTransfer = new OriginalTransfer(tranId, requestConfiguration.GetPrimaryPurse(),
                requestConfiguration.GetSecondaryPurse(), (Amount)0.01M)
            {
                Description = (Description)"Проверка завершения перевода с кодом протекции",
                Period = 1,
                Code = protectionCode,
                Force = true
            };

            var recentTransfer = originalTransfer.Submit();

            Assert.AreEqual(TransferType.Protection, recentTransfer.Transfer.TransferType);

            // X5. Завершение операции с протекцией.
            var protectionFinisher = new ProtectionFinisher(recentTransfer.Transfer.PrimaryId, protectionCode);
            var protectionReport = protectionFinisher.Submit();

            Assert.AreEqual(TransferType.Normal, protectionReport.TransferType);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateTransferWithProtection_RejectProtection_ReturnsProtectionReport(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var protectionCode = (Description)"12345";

            // X2. Перевод средств.
            var tranId = requestConfiguration.GetUniqueId();
            var originalTransfer = new OriginalTransfer(tranId, requestConfiguration.GetPrimaryPurse(),
                requestConfiguration.GetSecondaryPurse(), (Amount)0.01M)
            {
                Description = (Description)"Проверка отмены перевода с кодом протекции",
                Period = 1,
                Code = protectionCode,
                Force = true
            };

            var recentTransfer = originalTransfer.Submit();

            Assert.AreEqual(TransferType.Protection, recentTransfer.Transfer.TransferType);

            // X12. Возврат платежа с протекцией.
            var protectionRejector = new ProtectionRejector(recentTransfer.Transfer.PrimaryId);
            var protectionReport = protectionRejector.Submit();
            
            Assert.AreEqual(TransferType.ProtectionCancel, protectionReport.TransferType);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateTransfer_RejectTransfer_ReturnsMoneybackReport(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // X2. Перевод средств.
            var tranId = requestConfiguration.GetUniqueId();
            var originalTransfer = new OriginalTransfer(tranId, requestConfiguration.GetPrimaryPurse(),
                requestConfiguration.GetSecondaryPurse(), (Amount)0.01M)
            {
                Description = (Description)"Проверка бескомиссионного возврата средств",
                Force = true
            };

            var recentTransfer = originalTransfer.Submit();

            //  X14. Бескомиссионный возврат средств.
            var transferRejector =
                new TransferRejector(recentTransfer.Transfer.PrimaryId, recentTransfer.Transfer.Amount);
            var moneybackReport = transferRejector.Submit();

            if (!moneybackReport.Description.ToString().StartsWith("Moneyback transaction WMTranId"))
                Assert.Fail("!moneybackReport.Description.ToString().StartsWith(\"Moneyback transaction WMTranId\")");
        }
    }
}
