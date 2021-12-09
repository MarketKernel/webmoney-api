using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X17. Operations with arbitration contracts.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class RecentContract : WmResponse
    {
        /// <summary>
        /// Number of the created contract.
        /// </summary>
        public int ContractId { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            ContractId = wmXmlPackage.SelectInt32("contractid");
        }
    }
}