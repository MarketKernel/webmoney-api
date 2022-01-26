//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using WebMoney.XmlInterfaces.BasicObjects;
//using WebMoney.XmlInterfaces.Tests.Internal;

//namespace WebMoney.XmlInterfaces.Tests
//{
//    [TestClass]
//    public sealed class ExpressTrustTests
//    {
//        static ExpressTrustTests()
//        {
//            StaticContextInitializer.Init();
//        }

//        // TODO: не удалось проверить (не настроен прием оплаты через Merchant).
//        [TestMethod]
//        [DataRow(AuthorizationMode.Classic)]
//        public void CreateExpressTrust_ReturnsExpressTrustReport(AuthorizationMode authorizationMode)
//        {
//            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
//            requestConfiguration.ApplyInitializer();

//            //    var expressTrustRequest = new ExpressTrustRequest(
//            //        _storePurse,
//            //        (Amount)0.01,
//            //        (Amount)0.0,
//            //        (Amount)0.0,
//            //        _clientPhone,
//            //        ConfirmationType.SMS,
//            //        CultureInfo.CreateSpecificCulture("ru-RU"));

//            //    var expressTrustResponse = expressTrustRequest.Submit();

//            //    Console.WriteLine(expressTrustResponse.Info);

//            //    string confirmationCode = null;

//            //    if (ConfirmationType.SMS == expressTrustResponse.ConfirmationType)
//            //        confirmationCode = Console.ReadLine();

//            //    var expressTrustConfirmation = new ExpressTrustConfirmation(
//            //        expressTrustResponse.Reference, confirmationCode, CultureInfo.CreateSpecificCulture("ru-RU"));

//            //    var expressTrustReport = expressTrustConfirmation.Submit();

//            //    Console.WriteLine(expressTrustReport.TrustId);
//        }
//    }
//}
