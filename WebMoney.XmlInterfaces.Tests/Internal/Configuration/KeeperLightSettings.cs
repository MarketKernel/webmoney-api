using System.Security.Cryptography.X509Certificates;

namespace WebMoney.XmlInterfaces.Tests.Internal
{
    internal class KeeperLightSettings
    {
        public StoreLocation StoreLocation { get; set; }
        public StoreName StoreName { get; set; }
        public string Thumbprint { get; set; } = null!;
    }
}
