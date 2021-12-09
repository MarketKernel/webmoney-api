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
    public class Transfer : Operation
    {
        /// <summary>
        /// Sender’s purse number.
        /// </summary>
        public Purse SourcePurse { get; protected set; }

        /// <summary>
        /// Fee charged for the transfer.
        /// </summary>
        public Amount Commission { get; protected set; }

        /// <summary>
        /// Transfer type.
        /// </summary>
        public TransferType TransferType { get; protected set; }

        /// <summary>
        /// Invoice number (in the WebMoney system) of the transaction.
        /// </summary>
        public long InvoiceId { get; protected set; }

        /// <summary>
        /// Invoice number set by the merchant.
        /// </summary>
        public int OrderId { get; protected set; }

        /// <summary>
        /// Transaction number. Transaction number set by the sender. It should be unique for each transaction (the same tranid may not be used for two transactions).
        /// </summary>
        public int PaymentId { get; protected set; }

        /// <summary>
        /// Protection period in days. 0 - means that protection is disabled.
        /// </summary>
        public byte Period { get; protected set; }

        /// <summary>
        /// Correspondent’s WMID.
        /// </summary>
        public WmId Partner { get; protected set; }

        /// <summary>
        /// Balance after transaction. For protected transactions, the sender's balance is displayed as of the start of the transaction, and for the recipient – as of the end of the transaction.
        /// If the transaction has not been completed yet, the balance is displayed as of the moment the transaction started.
        /// </summary>
        public Amount Rest { get; protected set; }

        /// <summary>
        /// Incomplete transaction with time protection. Tag is present, only if the transaction has not been completed yet.
        /// </summary>
        public bool IsLocked { get; protected set; }

        internal override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("@id");
            SecondaryId = wmXmlPackage.SelectInt64("@ts");
            SourcePurse = wmXmlPackage.SelectPurse("pursesrc");
            TargetPurse = wmXmlPackage.SelectPurse("pursedest");
            Amount = wmXmlPackage.SelectAmount("amount");
            Commission = wmXmlPackage.SelectAmount("comiss");
            TransferType = (TransferType)wmXmlPackage.SelectInt32("opertype");
            InvoiceId = wmXmlPackage.SelectInt64("wminvid");
            OrderId = wmXmlPackage.SelectInt32("orderid");
            PaymentId = wmXmlPackage.SelectInt32("tranid");
            Period = wmXmlPackage.SelectUInt8("period");
            Description = (Description)wmXmlPackage.SelectString("desc");
            CreateTime = wmXmlPackage.SelectWmDateTime("datecrt");
            UpdateTime = wmXmlPackage.SelectWmDateTime("dateupd");
            Partner = wmXmlPackage.SelectWmId("corrwm");
            Rest = wmXmlPackage.SelectAmount("rest");

            if (wmXmlPackage.Exists("timelock"))
                IsLocked = true;
        }
    }
}