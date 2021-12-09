using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X4. Receiving the history of issued invoices. Verifying whether invoices were paid.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OutgoingInvoiceFilter : WmRequest<OutgoingInvoiceRegister>
    {
        private long _invoiceId;
        private int _orderId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLOutInvoices.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLOutInvoicesCert.asp";

        /// <summary>
        /// Number of the purse for which the invoice was issued.
        /// </summary>
        public Purse Purse { get; set; }

        /// <summary>
        /// Invoice number (in the WebMoney system).
        /// </summary>
        public long InvoiceId
        {
            get => _invoiceId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _invoiceId = value;
            }
        }

        /// <summary>
        /// Serial invoice number. Serial invoice number set by the sender.
        /// </summary>
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _orderId = value;
            }
        }

        /// <summary>
        /// Minimum date and time of invoice creation.
        /// </summary>
        public WmDateTime StartTime { get; set; }

        /// <summary>
        /// Maximum date and time of invoice creation.
        /// </summary>
        public WmDateTime FinishTime { get; set; }

        protected internal OutgoingInvoiceFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purse">Number of the purse for which the invoice was issued.</param>
        /// <param name="startTime">Minimum date and time of invoice creation.</param>
        /// <param name="finishTime"> Maximum date and time of invoice creation.</param>
        public OutgoingInvoiceFilter(Purse purse, WmDateTime startTime, WmDateTime finishTime)
        {
            Purse = purse;
            StartTime = startTime;
            FinishTime = finishTime;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", Purse, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("getoutinvoices"); // <getoutinvoices>

            xmlRequestBuilder.WriteElement("purse", Purse.ToString());
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);
            xmlRequestBuilder.WriteElement("orderid", OrderId);
            xmlRequestBuilder.WriteElement("datestart", StartTime.ToServerString());
            xmlRequestBuilder.WriteElement("datefinish", FinishTime.ToServerString());

            xmlRequestBuilder.WriteEndElement(); // </getoutinvoices>
        }
    }
}