using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using WebMoney.Cryptography;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.XmlInterfaces.Tests.Internal
{
    internal class RequestConfiguration
    {
        private static int _lastId;

        public ulong PrimaryWmId { get; set; }
        public ulong SecondaryWmId { get; set; }

        public string PrimaryPurse { get; set; } = null!;
        public string SecondaryPurse { get; set; } = null!;

        public string ClientFirstName { get; set; } = null!;
        public string ClientSecondName { get; set; } = null!;
        public string ClientPatronymic { get; set; } = null!;

        public string ClientPassportNumber { get; set; } = null!;

        public string ClientPhone { get; set; } = null!;

        public string ClientBank { get; set; } = null!;
        public string ClientBankCard { get; set; } = null!;
        public string ClientBankAccount { get; set; } = null!;

        public AuthorizationSettings AuthorizationSettings { get; set; } = null!;

        public int GetUniqueId()
        {
            // TODO: Проблема 2038 года!
            var id = (int) DateTimeOffset.Now.ToUnixTimeSeconds();

            if (_lastId >= id)
                id = _lastId + 1;

            _lastId = id;

            return id;
        }

        public WmId GetPrimaryWmId()
        {
            return (WmId) PrimaryWmId;
        }

        public WmId GetSecondaryWmId()
        {
            return (WmId)SecondaryWmId;
        }

        public Purse GetPrimaryPurse()
        {
            return Purse.Parse(PrimaryPurse);
        }

        public Purse GetSecondaryPurse()
        {
            return Purse.Parse(SecondaryPurse);
        }

        public void ApplyInitializer()
        {
            Initializer initializer = GetInitializer();
            initializer.Apply();
        }

        public Initializer GetInitializer()
        {
            Initializer initializer;

            var authorizationSettings = AuthorizationSettings;

            switch (authorizationSettings.AuthorizationMode)
            {
                case AuthorizationMode.Merchant:
                    var merchantSettings = authorizationSettings.MerchantSettings;

                    if (null == merchantSettings)
                        throw new InvalidOperationException("null == merchantSettings");

                    initializer = new Initializer((WmId)merchantSettings.WmId, merchantSettings.SecretKey);
                    break;
                case AuthorizationMode.Classic:
                    var keeperClassicSettings = authorizationSettings.KeeperClassicSettings;

                    if (null == keeperClassicSettings)
                        throw new InvalidOperationException("null == keeperClassicSettings");

                    initializer = new Initializer((WmId)keeperClassicSettings.WmId,
                        new KeeperKey(keeperClassicSettings.KeeperKey));
                    break;
                case AuthorizationMode.Light:
                    var keeperLightSettings = authorizationSettings.KeeperLightSettings;

                    if (null == keeperLightSettings)
                        throw new InvalidOperationException("null == keeperLightSettings");

                    var certificate = GetCertificate(keeperLightSettings.StoreLocation, keeperLightSettings.StoreName,
                        keeperLightSettings.Thumbprint);

                    initializer = new Initializer(certificate);
                    break;
                default:
                    throw new InvalidOperationException("authorizationSettings.AuthorizationMode=" +
                                                        authorizationSettings.AuthorizationMode);
            }

            return initializer;
        }

        private static X509Certificate2 GetCertificate(StoreLocation storeLocation, StoreName storeName,
            string thumbprint)
        {
            X509Certificate2 certificate;
            var store = new X509Store(storeName, storeLocation);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                var collection =
                    store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (0 == collection.Count || !collection[0].HasPrivateKey)
                    throw new ConfigurationErrorsException(
                        "webMoneyConfiguration/applicationInterfaces/keeperLight/containerInfo/thumbprint");

                certificate = collection[0];
            }
            finally
            {
                store.Close();
            }

            return certificate;
        }
    }
}
