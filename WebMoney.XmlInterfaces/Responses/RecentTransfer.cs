using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X2. Transferring funds from one purse to another.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class RecentTransfer : Operation
    {
        /// <summary>
        /// Transaction number set in the sender’s accounting system; an integer; it should be unique for each transaction (the same tranid may not be used for two transactions).
        /// </summary>
        public int PaymentId { get; protected set; }

        /// <summary>
        /// Sender’s WM purse number.
        /// </summary>
        public Purse SourcePurse { get; protected set; }

        /// <summary>
        /// Fee charged.
        /// </summary>
        public Amount Commission { get; protected set; }

        /// <summary>
        /// Transfer type: simple or code-protected.
        /// </summary>
        public TransferType TransferType { get; protected set; }

        /// <summary>
        /// Protection period.
        /// </summary>
        public byte Period { get; protected set; }

        /// <summary>
        /// Invoice number (in the WebMoney system). Means that transfer is made without an invoice.
        /// </summary>
        public long InvoiceId { get; protected set; }

        internal override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("operation/@id");
            SecondaryId = wmXmlPackage.SelectInt64("operation/@ts");
            PaymentId = wmXmlPackage.SelectInt32("operation/tranid");
            SourcePurse = wmXmlPackage.SelectPurse("operation/pursesrc");
            TargetPurse = wmXmlPackage.SelectPurse("operation/pursedest");
            Amount = wmXmlPackage.SelectAmount("operation/amount");
            Commission = wmXmlPackage.SelectAmount("operation/comiss");
            TransferType = (TransferType)wmXmlPackage.SelectInt32("operation/opertype");
            Period = wmXmlPackage.SelectUInt8("operation/period");
            InvoiceId = wmXmlPackage.SelectInt64("operation/wminvid");
            Description = (Description)wmXmlPackage.SelectString("operation/desc");
            CreateTime = wmXmlPackage.SelectWmDateTime("operation/datecrt");
            UpdateTime = wmXmlPackage.SelectWmDateTime("operation/dateupd");
        }
    }
}