using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X3. Receiving Transaction History. Checking Transaction Status.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class TransferFilter : WmRequest<TransferRegister>
    {
        private long _transferId;
        private int _paymentId;
        private long _invoiceId;
        private int _orderId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLOperations.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLOperationsCert.asp";

        /// <summary>
        /// WMID purse number for which the transaction is requested.
        /// </summary>
        public Purse Purse { get; set; }

        /// <summary>
        /// Transaction number (in the WebMoney system).
        /// </summary>
        public long TransferId
        {
            get => _transferId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _transferId = value;
            }
        }

        /// <summary>
        /// Transfer number set by sender.
        /// </summary>
        public int PaymentId
        {
            get => _paymentId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _paymentId = value;
            }
        }

        /// <summary>
        /// Invoice number (in the WebMoney system) for which the transaction was made.
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
        /// Invoice number. Invoice number set by merchant.
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
        /// Minimum date and time of transaction execution.
        /// </summary>
        public WmDateTime StartTime { get; set; }

        /// <summary>
        /// Maximum date and time of transaction execution.
        /// </summary>
        public WmDateTime FinishTime { get; set; }

        protected internal TransferFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purse">WMID purse number for which the transaction is requested.</param>
        /// <param name="startTime">Minimum date and time of transaction execution.</param>
        /// <param name="finishTime">Maximum date and time of transaction execution.</param>
        public TransferFilter(Purse purse, WmDateTime startTime, WmDateTime finishTime)
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

            xmlRequestBuilder.WriteStartElement("getoperations"); // <getoperations>

            xmlRequestBuilder.WriteElement("purse", Purse.ToString());
            xmlRequestBuilder.WriteElement("wmtranid", TransferId);
            xmlRequestBuilder.WriteElement("tranid", PaymentId);
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);
            xmlRequestBuilder.WriteElement("orderid", OrderId);
            xmlRequestBuilder.WriteElement("datestart", StartTime.ToServerString());
            xmlRequestBuilder.WriteElement("datefinish", FinishTime.ToServerString());

            xmlRequestBuilder.WriteEndElement(); // </getoperations>
        }
    }
}