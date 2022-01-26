using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Tests.Internal;

namespace WebMoney.XmlInterfaces.Tests
{
    [TestClass]
    public sealed class MessageTests
    {
        static MessageTests()
        {
            StaticContextInitializer.Init();
        }
        
        [TestMethod]
        [DataRow(AuthorizationMode.Classic)]
        [DataRow(AuthorizationMode.Light)]
        public void CreateMessage_ReturnsRecentMessage(AuthorizationMode authorizationMode)
        {
            var requestConfiguration = Configurator.BuildRequestConfiguration(authorizationMode);
            requestConfiguration.ApplyInitializer();

            var subject = (Description)"Проверка";
            var content = (Message)"Текст сообщения (не более 1024 символов).";

            var originalMessage = new OriginalMessage(requestConfiguration.GetSecondaryWmId(), content)
            {
                Subject = subject
            };
            var recentMessage = originalMessage.Submit();

            Assert.AreEqual(content, recentMessage.Content);
        }
    }
}
