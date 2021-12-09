using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    ///  Interface X14. Fee-free refund.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class MoneybackReport : WmResponse
    {
        /// <summary>
        /// Unique number of a transaction in the WebMoney System.
        /// </summary>
        public long PrimaryId { get; protected set; }

        /// <summary>
        /// Service number of a transaction in the WebMoney System.
        /// </summary>
        public long SecondaryId { get; protected set; }

        /// <summary>
        /// Refund transaction number.
        /// </summary>
        public long TransferId { get; protected set; }

        /// <summary>
        /// The purse, from which the refund has been performed. It is the payee's purse, which received inwtranid transaction.
        /// </summary>
        public Purse SourcePurse { get; protected set; }

        /// <summary>
        /// The purse, to which the refund has been performed. It is the payee's purse, which sent the inwtranid transaction.
        /// </summary>
        public Purse TargetPurse { get; protected set; }

        /// <summary>
        /// Refund amount.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// Refund fee.
        /// </summary>
        public Amount Commission { get; protected set; }

        /// <summary>
        /// Refund description. Has the format "Moneyback transaction WMTranId: InWMTranID. (inwmtranid_desc)", where inwmtranid_desc is the original description of the inwmtranid transaction.
        /// </summary>
        public Description Description { get; protected set; }

        /// <summary>
        /// Date and time of the operation.
        /// </summary>
        public WmDateTime CreateTime { get; protected set; }

        /// <summary>
        /// Date and time of the last modification of the operation.
        /// </summary>
        public WmDateTime UpdateTime { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new TransferRejectorException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("operation/@id");
            SecondaryId = wmXmlPackage.SelectInt64("operation/@ts");
            TransferId = wmXmlPackage.SelectInt64("operation/inwmtranid");
            SourcePurse = wmXmlPackage.SelectPurse("operation/pursesrc");
            TargetPurse = wmXmlPackage.SelectPurse("operation/pursedest");
            Amount = wmXmlPackage.SelectAmount("operation/amount");
            Commission = wmXmlPackage.SelectAmount("operation/comiss");
            Description = (Description)wmXmlPackage.SelectString("operation/desc");
            CreateTime = wmXmlPackage.SelectWmDateTime("operation/datecrt");
            UpdateTime = wmXmlPackage.SelectWmDateTime("operation/dateupd");
        }
    }
}