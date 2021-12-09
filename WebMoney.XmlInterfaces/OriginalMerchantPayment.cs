using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X22. Receiving the ticket of prerequest payment form at merchant.webmoney
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalMerchantPayment : WmRequest<MerchantPaymentToken>
    {
        private const ushort MaxLifeTime = 744;

        private int _orderId;
        private ushort _lifetime;

        protected override string ClassicUrl => "https://merchant.webmoney.ru/conf/xml/XMLTransSave.asp";

        protected override string LightUrl => throw new NotSupportedException();

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

        public Purse StorePurse { get; set; }

        public Amount Amount { get; set; }

        public Description Description { get; set; }

        /// <summary>
        /// Ticket validity period. A positive integer defining the number of hours during which the ticket is valid. It can't exceed the period of 744 hours: in that case the ticket becomes invalid.
        /// The number of tickets with a specific validity period is unlimited.
        /// There is a possibility to create a 'timeless' ticket by defining the validity period as '0', but there can be only one such ticket generated for a WM-purse.
        /// If the interface is called repeatedly for the same purse and with null validity period, the previous ticket will be updated with the new data and without changing the ticket number.
        /// </summary>
        public ushort Lifetime
        {
            get => _lifetime;
            set
            {
                if (value > MaxLifeTime)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _lifetime = value;
            }
        }

        protected internal OriginalMerchantPayment()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="storePurse"></param>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        /// <param name="lifeTime">Ticket validity period.</param>
        public OriginalMerchantPayment(
            int orderId, Purse storePurse, Amount amount, Description description, ushort lifeTime)
        {
            if (orderId < 0)
                throw new ArgumentOutOfRangeException(nameof(orderId));

            if (lifeTime > MaxLifeTime)
                throw new ArgumentOutOfRangeException(nameof(lifeTime));

            OrderId = orderId;
            StorePurse = storePurse;
            Amount = amount;
            Description = description;
            Lifetime = lifeTime;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(
                CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Initializer.Id, StorePurse, OrderId, Lifetime);
        }

        protected override void BuildXmlHead(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder) throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartDocument();

            xmlRequestBuilder.WriteStartElement("merchant.request"); // <merchant.request>

            xmlRequestBuilder.WriteStartElement("signtags"); // <signtags>

            xmlRequestBuilder.WriteElement("wmid", Initializer.Id.ToString());
            xmlRequestBuilder.WriteElement("validityperiodinhours", Lifetime);

            ulong requestNumber = Initializer.GetRequestNumber();

            switch (Initializer.Mode)
            {
                case AuthorizationMode.Merchant:
                    xmlRequestBuilder.WriteElement(
                        "md5", Utilities.CryptographyUtility.ComputeHash(BuildMessage(requestNumber) + Initializer.SecretKey));
                    break;
                case AuthorizationMode.Classic:
                    xmlRequestBuilder.WriteElement("sign", Initializer.Sign(BuildMessage(requestNumber)));
                    break;
                default:
                    throw new InvalidOperationException("Initializer.Mode=" + Initializer.Mode);
            }

            xmlRequestBuilder.WriteEndElement(); // </signtags>
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("paymenttags"); // <paymenttags>

            xmlRequestBuilder.WriteElement("lmi_payee_purse", StorePurse.ToString());
            xmlRequestBuilder.WriteElement("lmi_payment_amount", Amount.ToString());
            xmlRequestBuilder.WriteElement("lmi_payment_no", OrderId);
            xmlRequestBuilder.WriteElement("lmi_payment_desc", Description);

            xmlRequestBuilder.WriteEndElement(); // </paymenttags>
        }

        protected override void BuildXmlFoot(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteEndElement(); // </merchant.request>

            xmlRequestBuilder.WriteEndDocument();
        }
    }
}
