using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X7. Verifying client's handwritten signature - owner of WM Keeper WinPro (Classic).
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class SignatureEvidence : WmResponse
    {
        /// <summary>
        /// Authentication result.
        /// </summary>
        public bool VerificationResult { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            string result = wmXmlPackage.SelectNotEmptyString("testsign/res");
            VerificationResult = result.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}