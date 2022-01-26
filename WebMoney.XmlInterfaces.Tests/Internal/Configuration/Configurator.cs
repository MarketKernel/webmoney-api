using System;
using Microsoft.Extensions.Configuration;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.XmlInterfaces.Tests.Internal
{
    internal static class Configurator
    {
        private const string ConfigurationDirectory = @"D:\_WORK\WM\_TEST";

        private const string MerchantConfigurationFile = "MerchantConfiguration.json";
        private const string KeeperClassicConfigurationFile = "KeeperClassicConfiguration.json";
        private const string KeeperLightConfigurationFile = "KeeperLightConfiguration.json";

        private static RequestConfiguration? _merchantConfiguration;
        private static RequestConfiguration? _keeperClassicConfiguration;
        private static RequestConfiguration? _keeperLightConfiguration;

        public static RequestConfiguration BuildRequestConfiguration(AuthorizationMode authorizationMode)
        {
            switch (authorizationMode)
            {
                case AuthorizationMode.Merchant:
                    return _merchantConfiguration ??= BuildConfiguration(MerchantConfigurationFile);
                case AuthorizationMode.Classic:
                    return _keeperClassicConfiguration ??= BuildConfiguration(KeeperClassicConfigurationFile);
                case AuthorizationMode.Light:
                    return _keeperLightConfiguration ??= BuildConfiguration(KeeperLightConfigurationFile);
                default:
                    throw new ArgumentOutOfRangeException(nameof(authorizationMode), authorizationMode, null);
            }
        }

        private static RequestConfiguration BuildConfiguration(string configurationFile,
            string configurationDirectory = ConfigurationDirectory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(configurationDirectory)
                .AddJsonFile(configurationFile, false, false);

            var configuration = builder.Build();

            return configuration.Get<RequestConfiguration>();
        }
    }
}