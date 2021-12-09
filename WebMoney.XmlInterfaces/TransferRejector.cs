using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X14. Fee-free refund.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class TransferRejector : WmRequest<MoneybackReport>
    {
        private long _transferId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLTransMoneyback.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLTransMoneybackCert.asp";

        /// <summary>
        /// The transaction id. This tag contains the internal WebMoney id (positive integer) of the transaction (wmtranid) to be refunded. The transaction type must be - simple (opertype=0).
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
        /// The transaction amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Client phone number.
        /// </summary>
        public Phone Phone { get; set; }

        protected internal TransferRejector()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId">The transaction id.</param>
        /// <param name="amount">The transaction amount.</param>
        public TransferRejector(long transferId, Amount amount)
        {
            if (transferId < 0)
                throw new ArgumentOutOfRangeException(nameof(transferId));

            TransferId = transferId;
            Amount = amount;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", requestNumber, TransferId, Amount);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("trans"); // <trans>

            xmlRequestBuilder.WriteElement("inwmtranid", TransferId);
            xmlRequestBuilder.WriteElement("amount", Amount.ToString());
            xmlRequestBuilder.WriteElement("moneybackphone", Phone.ToString());

            xmlRequestBuilder.WriteEndElement(); // </trans>
        }
    }
}