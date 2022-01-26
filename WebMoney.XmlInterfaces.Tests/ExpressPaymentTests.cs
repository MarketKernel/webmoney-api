//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using WebMoney.XmlInterfaces.BasicObjects;
//using WebMoney.XmlInterfaces.Tests.Internal;

//namespace WebMoney.XmlInterfaces.Tests
//{
//    [TestClass]
//    public sealed class ExpressPaymentTests
//    {
//        static ExpressPaymentTests()
//        {
//            StaticContextInitializer.Init();
//        }

//        // TODO: не удалось проверить (похоже что check.webmoney.ru воспринимает телефон как WMU-счет).
//        [TestMethod]
//        [DataRow(AuthorizationMode.Merchant)]
//        public void CreateExpressPayment_ReturnsExpressPaymentReport(AuthorizationMode authorizationMode)
//        {
//            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
//            requestConfiguration.ApplyInitializer();

//            var expressPaymentRequest = new ExpressPaymentRequest(
//                requestConfiguration.GetSecondaryPurse(),
//                requestConfiguration.GetUniqueId(),
//                (Amount)0.01M,
//                (Description)"Проверка ExpressPayment",
//                Phone.Parse(requestConfiguration.ClientPhone),
//                ConfirmationType.SMS,
//                CultureInfo.GetCultureInfo("ru-RU"));

//            var expressPaymentResponse = expressPaymentRequest.Submit();

//            //    Console.WriteLine(expressPaymentResponse.Info);
//            //    Console.WriteLine(expressPaymentResponse.InvoiceId);

//            //    string confirmationCode = null;

//            //    if (ConfirmationType.SMS == expressPaymentResponse.ConfirmationType)
//            //        confirmationCode = Console.ReadLine();

//            //    var expressPaymentConfirmation = new ExpressPaymentConfirmation(
//            //        _storePurse,
//            //        confirmationCode,
//            //        expressPaymentResponse.InvoiceId,
//            //        CultureInfo.CreateSpecificCulture("ru-RU"));

//            //    string c1 = expressPaymentConfirmation.Compile();
//            //    Console.WriteLine(c1);

//            //    var expressPaymentReport = expressPaymentConfirmation.Submit();

//            //    Console.WriteLine(expressPaymentReport.SmsState);
//        }
//    }
//}
