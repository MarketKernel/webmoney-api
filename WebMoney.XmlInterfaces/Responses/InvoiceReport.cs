using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X23. Rejection of received invoices or cancellation of issued invoices.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class InvoiceReport : WmResponse
    {
        public long PrimaryId { get; protected set; }

        public long SecondaryId { get; protected set; }

        /// <summary>
        /// Invoice state.
        /// </summary>
        public InvoiceState State { get; protected set; }

        /// <summary>
        /// Date and time of invoice status change.
        /// </summary>
        public WmDateTime UpdateTime { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("ininvoice/@id");
            SecondaryId = wmXmlPackage.SelectInt64("ininvoice/@ts");
            State = (InvoiceState)wmXmlPackage.SelectUInt32("ininvoice/state");
            UpdateTime = wmXmlPackage.SelectWmDateTime("ininvoice/dateupd");
        }
    }
}