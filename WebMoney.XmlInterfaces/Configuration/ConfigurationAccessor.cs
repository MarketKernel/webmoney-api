using System.Configuration;

namespace WebMoney.XmlInterfaces.Configuration
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    internal static class ConfigurationAccessor
    {
        private const string SectionName = "webMoneyConfiguration/applicationInterfaces";
        
        private static AuthorizationSettings _authorizationSettings;
        
        public static AuthorizationSettings GetAuthorizationSettings()
        {
            AuthorizationSettings authorizationSettings = _authorizationSettings;

            if (null != authorizationSettings)
                return authorizationSettings;

            authorizationSettings = ConfigurationManager.GetSection(SectionName) as AuthorizationSettings;

            _authorizationSettings = authorizationSettings ?? throw new ConfigurationErrorsException(SectionName);

            return authorizationSettings;
        }
    }
}
