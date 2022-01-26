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
    public sealed class ClientInspectorTests
    {
        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForCashPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Ввод/вывод WM наличными в одном из обменных пунктов.
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), (Description)requestConfiguration.ClientPassportNumber,
                (Description) requestConfiguration.ClientSecondName,
                (Description) requestConfiguration.ClientFirstName)
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForOfflineSystemPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Ввод/вывод WM наличными через системы денежных переводов.
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), (Description) requestConfiguration.ClientSecondName,
                (Description) requestConfiguration.ClientFirstName)
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForBankAccountPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Ввод/вывод WM на банковский счет
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), (Description) requestConfiguration.ClientSecondName,
                (Description) requestConfiguration.ClientFirstName, (Description) requestConfiguration.ClientBank,
                BankAccount.Parse(requestConfiguration.ClientBankAccount))
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForBankCardPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Ввод/вывод WM на банковскую карту.
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), (Description) requestConfiguration.ClientSecondName,
                (Description) requestConfiguration.ClientFirstName, (Description) requestConfiguration.ClientBank,
                BankCard.Parse(requestConfiguration.ClientBankCard))
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForInternetSystemPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Обмен WM на электронную валюту других систем.
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), PaymentSystem.Qiwi, (Description) requestConfiguration.ClientPhone)
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void InspectClientForSmsPayment_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            // Ввод WM за SMS (только operation/direction=2).
            var clientInspector = new ClientInspector(WmCurrency.Z, (Amount) 0.01M,
                requestConfiguration.GetPrimaryWmId(), requestConfiguration.ClientPhone)
            {
                Output = false
            };

            var clientEvidence = clientInspector.Submit();

            Assert.IsNotNull(clientEvidence);
        }

        //[TestMethod]
        //[DataRow(AuthorizationMode.Classic)]
        //[DataRow(AuthorizationMode.Light)]
        //public void InspectClientForMobileTopUp_ReturnsClientEvidence(AuthorizationMode authorizationMode)
        //{
        //    var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
        //    requestConfiguration.ApplyInitializer();

        //    // Пополнение телефона.
        //    var clientInspector = new ClientInspector(WmCurrency.Z, (Amount)0.01M,
        //        requestConfiguration.GetPrimaryWmId(), requestConfiguration.ClientPhone, true)
        //    {
        //        Output = true
        //    };

        //    var clientEvidence = clientInspector.Submit();

        //    Assert.IsNotNull(clientEvidence);
        //}
    }
}
