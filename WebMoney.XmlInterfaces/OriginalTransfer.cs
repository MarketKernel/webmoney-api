using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X2. Transferring funds from one purse to another.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalTransfer : WmRequest<TransferEnvelope>
    {
        private int _paymentId;
        private long _invoiceId;

        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLTrans.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLTransCert.asp";

        /// <summary>
        /// Transaction number in the sender's accounting system; any positive integer; must be unique for the WMID that signs the request.
        /// It's not allowed to perform two transactions with the same 'tranid' from one WMID (from different purses of a WMID either).
        /// The uniqueness of tranid value is verified for the period not shorter than one year.
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
        /// Sender’s WM purse number.
        /// </summary>
        public Purse SourcePurse { get; set; }

        /// <summary>
        /// Recipient’s purse number.
        /// </summary>
        public Purse TargetPurse { get; set; }

        /// <summary>
        /// Amount of the sum transferred.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Protection period.
        /// </summary>
        public byte Period { get; set; }

        /// <summary>
        /// Protected payment. Arbitrary string of 5 to 255 characters. No spaces may be used at the beginning or the end.
        /// </summary>
        public Description Code { get; set; }

        /// <summary>
        /// Description of the purchased product or service.
        /// </summary>
        public Description Description { get; set; }

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
        /// obligatorily! False – the transfer will be made only if the recipient allows the transfer (otherwise the returned code will be - 35).
        /// The recepient can prohibit accepting payments in two cases.
        /// The first is when the sender is an authorized correspondent for the recepient for whom the latter had prohibited the possibility of making payments to his purses ('restrictions' section in the correspondent's properties).
        /// The second is when the sender isn't an authorized correspondent for the recepient, and the latter had prohibited the possibility of making payments to his purses for all unauthorized members.
        /// </summary>
        public bool Force { get; set; }

        protected internal OriginalTransfer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId">Transaction number in the sender's accounting system.</param>
        /// <param name="sourcePurse">Sender’s WM purse number.</param>
        /// <param name="targetPurse">Recipient’s purse number.</param>
        /// <param name="amount">Amount of the sum transferred.</param>
        public OriginalTransfer(int paymentId, Purse sourcePurse, Purse targetPurse, Amount amount)
        {
            if (paymentId < 0)
                throw new ArgumentOutOfRangeException(nameof(paymentId));

            PaymentId = paymentId;
            SourcePurse = sourcePurse;
            TargetPurse = targetPurse;
            Amount = amount;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6}{7}{8}", requestNumber, PaymentId,
                SourcePurse, TargetPurse, Amount, Period, Code, Description, InvoiceId);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("trans"); // <trans>

            xmlRequestBuilder.WriteElement("tranid", PaymentId);
            xmlRequestBuilder.WriteElement("pursesrc", SourcePurse.ToString());
            xmlRequestBuilder.WriteElement("pursedest", TargetPurse.ToString());
            xmlRequestBuilder.WriteElement("amount", Amount.ToString());
            xmlRequestBuilder.WriteElement("period", Period);
            xmlRequestBuilder.WriteElement("pcode", Code);
            xmlRequestBuilder.WriteElement("desc", Description);
            xmlRequestBuilder.WriteElement("wminvid", InvoiceId);
            xmlRequestBuilder.WriteElement("onlyauth", Force ? 0 : 1);

            xmlRequestBuilder.WriteEndElement(); // </trans>

            xmlRequestBuilder.WriteElement("wmb_denomination", 1);
        }
    }
}