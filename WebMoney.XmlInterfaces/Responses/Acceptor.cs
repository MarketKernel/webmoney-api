using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Information about one acceptor.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class Acceptor
    {
        /// <summary>
        /// Contract number.
        /// </summary>
        public int ContractId { get; protected set; }

        /// <summary>
        /// Acceptor's WMID.
        /// </summary>
        public WmId WmId { get; protected set; }

        /// <summary>
        /// Date and time of acceptance. For example 2005-11-29T12:00:39.077, if there is no acceptance date, then this user hasn't accepted this contract.
        /// </summary>
        public WmDateTime? AcceptTime { get; protected set; }

        internal void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            ContractId = wmXmlPackage.SelectInt32("@contractid");
            WmId = wmXmlPackage.SelectWmId("@wmid");
            AcceptTime = wmXmlPackage.SelectWmDateTimeIfExists("@acceptdate");    
        }
    }
}