using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X23. Rejection of received invoices or cancellation of issued invoices.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class InvoiceRefusal : WmRequest<InvoiceReport>
    {
        private long _invoiceId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLInvoiceRefusal.asp";

        //protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLInvoiceRefusalCert.asp";
        protected override string LightUrl => throw new NotSupportedException();

        /// <summary>
        /// Sender`s or receipt`s WMID.
        /// </summary>
        public WmId WmId { get; set; }

        /// <summary>
        /// The invoice's unique number in the WebMoney system; corresponds to id attribute of the parameter "invoice", response to the request on the Interface X1 (Sending invoice).
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

        protected internal InvoiceRefusal()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">Sender`s or receipt`s WMID.</param>
        /// <param name="invoiceId">The invoice's unique number in the WebMoney system.</param>
        public InvoiceRefusal(WmId wmId, long invoiceId)
        {
            if (invoiceId < 0)
                throw new ArgumentOutOfRangeException(nameof(invoiceId));

            WmId = wmId;
            InvoiceId = invoiceId;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", WmId, InvoiceId, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("invoicerefuse"); // <invoicerefuse>

            xmlRequestBuilder.WriteElement("wmid", WmId.ToString());
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);

            xmlRequestBuilder.WriteEndElement(); // </invoicerefuse>
        }
    }
}