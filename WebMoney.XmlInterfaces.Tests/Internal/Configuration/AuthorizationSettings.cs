using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.XmlInterfaces.Tests.Internal
{
    internal class AuthorizationSettings
    {
        public AuthorizationMode AuthorizationMode { get; set; }

        public KeeperClassicSettings? KeeperClassicSettings { get; set; }
        
        public KeeperLightSettings? KeeperLightSettings { get; set; }

        public MerchantSettings? MerchantSettings { get; set; }
    }
}
