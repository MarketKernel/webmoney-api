using System;
using System.Security.Cryptography.X509Certificates;
using WebMoney.XmlInterfaces.Core;

namespace WebMoney.XmlInterfaces.Tests.Internal
{
    internal static class StaticContextInitializer
    {
        // Корневой сертификат WebMoney: годен до марта 2035
        private static readonly X509Certificate2 _webMoneyRootCertificate =
            new X509Certificate2(
                Convert.FromBase64String(
                    "MIIFsTCCA5mgAwIBAgIQA7dHzSZ7uJdBxFycIWn+WjANBgkqhkiG9w0BAQUFADBrMSswKQYDVQQLEyJXTSBUcmFuc2ZlciBDZXJ0aWZpY2F0aW9uIFNlcnZpY2VzMRgwFgYDVQQKEw9XTSBUcmFuc2ZlciBMdGQxIjAgBgNVBAMTGVdlYk1vbmV5IFRyYW5zZmVyIFJvb3QgQ0EwHhcNMTAwMzEwMTczNDU2WhcNMzUwMzEwMTc0NDUxWjBrMSswKQYDVQQLEyJXTSBUcmFuc2ZlciBDZXJ0aWZpY2F0aW9uIFNlcnZpY2VzMRgwFgYDVQQKEw9XTSBUcmFuc2ZlciBMdGQxIjAgBgNVBAMTGVdlYk1vbmV5IFRyYW5zZmVyIFJvb3QgQ0EwggIiMA0GCSqGSIb3DQEBAQUAA4ICDwAwggIKAoICAQDFLJXtzEkZxLj1HIj9EhGvajFJ7RCHzF+MK2ZrAgxmmOafiFP6QD/aVjIexBqRb8SVy38xH+wthqkZgLMOGn8uDNpFieEMoX3ZRfqtCcD76KDySTOX1QUwHAzBfGuhe2ZQULUIjxdPRa4NDyvmXh4pE/s1+/7dGbUZs/JpYYaD2xxAt5PDTjylsKOk4FMb5kv6jzORkXku5UKFGUXEXbbf1xzgYHMIzoeJGn+iPgVFYAvkkQyvxEaVj0lNE+q/ua761krgCo47BiH1zMFzkv4uNHEZfe/lyHaozzbsu6yaK3EdrURSLuWrlxKy9yo3xDe9TPkzkhPeJPbV7YgvUUtWSeAJpksBU8GCALEhSgXOfHckuJFj9QB3YecHBvjdSiAUuntwM/iHvtSOXEUHxqW75E2Gq/2L4vBcxArXVdbUrVQDF3klzYu17OFgfe1hHHMHzgr4HBMLZiRCcvNLqghBCVxu1DM15YDfw+wnNV/5dUPx60tiocmCZpJKTwVl8gc85QCPyREujey8F0kgdgssQosPWTTWDg7X4Ifw20VkplHZDr29K5HdwLe56TvOI/4H24XJdqpAxoLBx9PL6ZXxH52wU0bSluL8/joXGzavFrhsXH7jJocH6tsFVzBZrmnVswbUMHDNL3xSnr5fAAXXZa7UwHd3pq/fsdG7s9PByQIDAQABo1EwTzALBgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAdBgNVHQ4EFgQUsTCnSwOZT4Q2HBN9V/TrafuIG8MwEAYJKwYBBAGCNxUBBAMCAQAwDQYJKoZIhvcNAQEFBQADggIBAAy5jHDFpVWtF209N30I+LHsiqMaLUmYDeV6sUBJqmWAZav7pWnigiMLuJd9yRa/ow6yKlKPRi3sbKaBwsAQ+xnz811nLFBBdS4PkwlHu1B7P4B2YbcqmF6k1QieJBZxOn7wledtnoBAkZ4d6HEW1OM5cvCoyj8YAdJTZIBzn61aNt/viPvypIUQf6Ps6Q2daNEAj7DoxIY8crnOaSIGdGmlT/y/edSqWv9Am5e9KXkJhQWMnGXh43wJYyHTetxVWPS43bW7gIUADYycKSH3isrBN5xQOFXMfL+lVHHSs7ap23DOo7xIDenm5PWz+QdDDFz3zLVeRovnkIdka/Wgk3f6rFfKB0y5POJ+BJvkorIYNZiN3dnmc6cDP840BUMv3BUrOe8iSy5lRr8mR+daktbZfO8E/rAb3zEdN+KG/CNJfAnQvp6DT4LqY/J9pG+VusH5GpUwuXr7UqLwEnd1LRp7qm28Cic7fegUnnUpkuF4ZFq8pWq8w59sOWlRuKBuWX46OghMrjgD0AN1hlA2/d5ULImX70Q2te3xiS1vrQhu77mkb/jA4/9+YfeT7VMpbnC3OoHiZ2bjudKnthlOs+AuUvzB4Tqo62VSF5+r0sYI593S+STmaZBAzsoaoEB7qxqKbEKCvXb9BlXkL76xIOEkbSIdPIkGXM4aMo4mTVz7"));

        private static bool _initializationCompleted = false;

        public static void Init()
        {
            if (_initializationCompleted)
                return;

            CertificateValidator.RegisterTrustedCertificate(_webMoneyRootCertificate);

            _initializationCompleted = true;
        }
    }
}
