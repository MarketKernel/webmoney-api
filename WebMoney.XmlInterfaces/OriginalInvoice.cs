using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X1. Sending invoice from merchant to customer.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalInvoice : WmRequest<InvoiceEnvelope>
    {
        private int _orderId;
        private int? _shopId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLInvoice.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLInvoiceCert.asp";

        /// <summary>
        /// Number of the invoice in the merchant's accounting system. An integer without a decimal point.
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
        /// Customer’s WMID.
        /// </summary>
        public WmId SourceWmId { get; set; }

        /// <summary>
        /// The number of the purse which the invoice should be paid to.
        /// </summary>
        public Purse TargetPurse { get; set; }

        /// <summary>
        /// Invoice amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// Description of the product or service.
        /// </summary>
        public Description Description { get; set; }

        /// <summary>
        /// Delivery address.
        /// </summary>
        public Description Address { get; set; }

        /// <summary>
        /// Maximum protection period allowed in days; An integer in the range 0 - 255; zero means that protection is prohibited. When issuing an invoice to a WM purse, this is the maximum period (in days) for returning funds.
        /// </summary>
        public byte Period { get; set; }

        /// <summary>
        /// Maximum valid payment period in days; an integer in the range 0 - 255; zero means that the period is undefined.
        /// </summary>
        public byte Expiration { get; set; }

        /// <summary>
        /// If True - then invoices are issued without considering recipient's permission for this action. If False - then invoices can be issued only with recipient's permission (otherwise error code 35 is returned). Users can forbit to issue them invoices in two cases. The first case is when the recipient forbade to issue him invoices for this specific correspondent. The second case is when the recipient forbade to issue him invoices for unauthorized correspondents, and the issuer is unauthorized.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// This parameter is obligatory for aggregators only ( these are transitional services that accept payments for third parties).
        /// This field should be used by aggregators to transmit the Megastock catalogue http://www.megastock.ru/ registration number of a store which the given payment is accepted for.
        /// </summary>
        public int? ShopId
        {
            get => _shopId;
            set
            {
                if (null != value && value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _shopId = value;
            }
        }

        protected internal OriginalInvoice()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">Number of the invoice in the merchant's accounting system. An integer without a decimal point.</param>
        /// <param name="sourceWmId">Customer’s WMID.</param>
        /// <param name="targetPurse">The number of the purse which the invoice should be paid to.</param>
        /// <param name="amount">Invoice amount.</param>
        public OriginalInvoice(int orderId, WmId sourceWmId, Purse targetPurse, Amount amount)
        {
            if (orderId < 0)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            OrderId = orderId;
            SourceWmId = sourceWmId;
            TargetPurse = targetPurse;
            Amount = amount;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}{5}{6}{7}{8}", OrderId,
                                     SourceWmId, TargetPurse, Amount, Description, Address, Period, Expiration,
                                     requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("invoice"); // <invoice>

            xmlRequestBuilder.WriteElement("orderid", OrderId);
            xmlRequestBuilder.WriteElement("customerwmid", SourceWmId.ToString());
            xmlRequestBuilder.WriteElement("storepurse", TargetPurse.ToString());
            xmlRequestBuilder.WriteElement("amount", Amount.ToString());
            xmlRequestBuilder.WriteElement("desc", Description);
            xmlRequestBuilder.WriteElement("address", Address);
            xmlRequestBuilder.WriteElement("period", Period);
            xmlRequestBuilder.WriteElement("expiration", Expiration);
            xmlRequestBuilder.WriteElement("onlyauth", Force ? 0 : 1);

            if (null != ShopId)
                xmlRequestBuilder.WriteElement("lmi_shop_id", ShopId.Value);

            xmlRequestBuilder.WriteEndElement(); // </invoice>
        }
    }
}