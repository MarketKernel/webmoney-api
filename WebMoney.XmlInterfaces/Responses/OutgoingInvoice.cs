using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OutgoingInvoice : Invoice
    {
        /// <summary>
        /// Customer's WMID.
        /// </summary>
        public WmId SourceWmId { get; protected set; }

        /// <summary>
        /// Transaction number in the WebMoney system(if the invoice has been paid).
        /// </summary>
        public long TransferId { get; protected set; }

        /// <summary>
        /// Payer's purse (if the invoice has been paid).
        /// </summary>
        public Purse? SourcePurse { get; protected set; }

        internal override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("@id");
            SecondaryId = wmXmlPackage.SelectInt64("@ts");
            OrderId = wmXmlPackage.SelectInt32("orderid");
            SourceWmId = wmXmlPackage.SelectWmId("customerwmid");
            TargetPurse = wmXmlPackage.SelectPurse("storepurse");
            Amount = wmXmlPackage.SelectAmount("amount");
            Description = (Description)wmXmlPackage.SelectString("desc");
            Address = (Description)wmXmlPackage.SelectString("address");
            Period = wmXmlPackage.SelectUInt8("period");
            Expiration = wmXmlPackage.SelectUInt8("expiration");
            InvoiceState = (InvoiceState)wmXmlPackage.SelectInt32("state");
            CreateTime = wmXmlPackage.SelectWmDateTime("datecrt");
            UpdateTime = wmXmlPackage.SelectWmDateTime("dateupd");
            TransferId = wmXmlPackage.SelectInt64("wmtranid");
            SourcePurse = wmXmlPackage.SelectPurseIfExists("customerpurse");
        }
    }
}