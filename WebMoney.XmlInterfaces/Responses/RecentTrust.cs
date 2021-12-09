using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X15. Viewing and changing settings of "by trust" management.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class RecentTrust : WmResponse
    {
        /// <summary>
        /// Unique trust number in WebMoney system.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Allows (if True) the identifier specified in master tag to issue invoices to the trusted purse specified purse or not (if False).
        /// </summary>
        public bool InvoiceAllowed { get; protected set; }

        /// <summary>
        /// Allows (if True) the identifier specified in master tag to transfer funds by trust from the trusted purse sepcified in purse or not (if False).
        /// </summary>
        public bool TransferAllowed { get; protected set; }

        /// <summary>
        ///  Allows (if True) the identifier specified in 'master' tag to view the account balance of the trusted purse specified in 'purse' or not (if False).
        /// </summary>
        public bool BalanceAllowed { get; protected set; }

        /// <summary>
        /// Allows (if True) the identifier specified in 'master' tag to view the history of operations of the purse specified in 'purse' or not (if False).
        /// </summary>
        public bool HistoryAllowed { get; protected set; }

        /// <summary>
        /// WM-identifier for which it is forbidden (allowed) to perform any operations with the purse specified in purse.
        /// </summary>
        public WmId Master { get; protected set; }

        /// <summary>
        /// The purse, any operations with which are forbidden (allowed) to the identifier specified in master.
        /// </summary>
        public Purse? Purse { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = wmXmlPackage.SelectInt32("trust/@id");
            InvoiceAllowed = wmXmlPackage.SelectBool("trust/@inv");
            TransferAllowed = wmXmlPackage.SelectBool("trust/@trans");
            BalanceAllowed = wmXmlPackage.SelectBool("trust/@purse");
            HistoryAllowed = wmXmlPackage.SelectBool("trust/@transhist");
            Master = wmXmlPackage.SelectWmId("trust/master");
            Purse = wmXmlPackage.SelectPurseIfExists("trust/purse");
        }
    }
}
