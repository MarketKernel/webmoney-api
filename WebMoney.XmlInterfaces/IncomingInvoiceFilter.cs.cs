using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X10. Retrieving list of invoices for payment.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class IncomingInvoiceFilter : WmRequest<IncomingInvoiceRegister>
    {
        private long _invoiceId;

        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLInInvoices.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLInInvoicesCert.asp";

        /// <summary>
        /// WM-identifier, for which for which invoice(s) for payment was(were) drawn.
        /// </summary>
        public WmId WmId { get; set; }

        /// <summary>
        /// Invoice number (in WebMoney system).
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
        /// Min time and date of invoice creation.
        /// </summary>
        public WmDateTime StartTime { get; set; }

        /// <summary>
        /// Max time and date of invoice creation.
        /// </summary>
        public WmDateTime FinishTime { get; set; }

        protected internal IncomingInvoiceFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">WM-identifier, for which for which invoice(s) for payment was(were) drawn.</param>
        /// <param name="startTime">Min time and date of invoice creation.</param>
        /// <param name="finishTime">Max time and date of invoice creation.</param>
        public IncomingInvoiceFilter(WmId wmId, WmDateTime startTime, WmDateTime finishTime)
        {
            WmId = wmId;
            StartTime = startTime;
            FinishTime = finishTime;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}", WmId, InvoiceId,
                                     StartTime.ToServerString(), FinishTime.ToServerString(), requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("getininvoices"); // <getininvoices>

            xmlRequestBuilder.WriteElement("wmid", WmId.ToString());
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);
            xmlRequestBuilder.WriteElement("datestart", StartTime.ToServerString());
            xmlRequestBuilder.WriteElement("datefinish", FinishTime.ToServerString());

            xmlRequestBuilder.WriteEndElement(); // </getininvoices>
        }
    }
}