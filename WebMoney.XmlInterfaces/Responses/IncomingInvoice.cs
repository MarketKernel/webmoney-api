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
    public class IncomingInvoice : Invoice
    {
        /// <summary>
        /// WMID of the seller who drew the invoice.
        /// </summary>
        public WmId TargetWmId { get; protected set; }

        /// <summary>
        /// Transaction number in the WebMoney system, if the invoice has been already paid.
        /// </summary>
        public long TransferId { get; protected set; }

        internal override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("@id");
            SecondaryId = wmXmlPackage.SelectInt64("@ts");
            OrderId = wmXmlPackage.SelectInt32("orderid");
            TargetWmId = wmXmlPackage.SelectWmId("storewmid");
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
        }
    }
}