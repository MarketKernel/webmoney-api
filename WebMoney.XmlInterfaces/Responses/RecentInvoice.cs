using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X1. Sending invoice from merchant to customer.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class RecentInvoice : Invoice
    {
        /// <summary>
        /// Customer’s WMID.
        /// </summary>
        public WmId SourceWmId { get; protected set; }

        internal override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("invoice/@id");
            SecondaryId = wmXmlPackage.SelectInt64("invoice/@ts");
            OrderId = wmXmlPackage.SelectInt32("invoice/orderid");
            SourceWmId = wmXmlPackage.SelectWmId("invoice/customerwmid");
            TargetPurse = wmXmlPackage.SelectPurse("invoice/storepurse");
            Amount = wmXmlPackage.SelectAmount("invoice/amount");
            Description = (Description)wmXmlPackage.SelectString("invoice/desc");
            Address = (Description)wmXmlPackage.SelectString("invoice/address");
            Period = wmXmlPackage.SelectUInt8("invoice/period");
            Expiration = wmXmlPackage.SelectUInt8("invoice/expiration");
            InvoiceState = (InvoiceState)wmXmlPackage.SelectInt32("invoice/state");
            CreateTime = wmXmlPackage.SelectWmDateTime("invoice/datecrt");
            UpdateTime = wmXmlPackage.SelectWmDateTime("invoice/dateupd");
        }
    }
}