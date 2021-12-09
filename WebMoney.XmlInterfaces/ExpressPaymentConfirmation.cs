using System;
using System.Globalization;
using System.Threading;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// X20 interface. Making transactions through the merchant.webmoney service without leaving the seller's site (resource, service, application).
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class ExpressPaymentConfirmation : WmRequest<ExpressPaymentReport>
    {
        private CultureInfo _culture;
        private long _invoiceId;

        protected override string ClassicUrl => "https://merchant.webmoney.ru/conf/xml/XMLTransConfirm.asp";

        protected override string LightUrl => throw new NotSupportedException();

        /// <summary>
        /// WM purse of the payment recipient. The number of a purse registered and configured to work in the merchant.webmoney service that will be used by the seller to accept payments.
        /// </summary>
        public Purse StorePurse { get; set; }

        /// <summary>
        /// Cliet code. This field contains a numeric code that the client receives to his/her mobile phone for payment confirmation.
        /// If no SMS is sent to the client (a USSD request is sent or the payment is expected to be made using a mobile purse management program), the value of the field should be 0. If an SMS message was sent to the client, but the client paid using a purse management application, setting this field to 0 will also be successful, since no code validation will be performed.
        /// If you pass -1 in this field and the payment is not made by the time the request is executed, the invoice will be canceled and the client won't be able to use it in the future.
        /// </summary>
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// WM invoice number. The number of the wminvoiceid invoice from the response to the previous request.
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
        /// Response language. This parameter transmits 'ru-RU' or 'en-US' value for Russian and English languages of the interface correspondingly. This value determines both the language in which SMS (USSD) requests are sent to the user and the language in which answers in 'userdec' tag are sent.
        /// </summary>
        public CultureInfo Culture
        {
            get => _culture;
            set => _culture = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected internal ExpressPaymentConfirmation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of the payment recipient.</param>
        /// <param name="confirmationCode">Cliet code.</param>
        /// <param name="invoiceId">WM invoice number.</param>
        /// <param name="culture">Response language.</param>
        public ExpressPaymentConfirmation(Purse storePurse, string confirmationCode, long invoiceId, CultureInfo culture = null)
        {
            if (null == culture)
                culture = Thread.CurrentThread.CurrentUICulture;

            StorePurse = storePurse;
            ConfirmationCode = confirmationCode ?? "0";
            InvoiceId = invoiceId;
            Culture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of the payment recipient.</param>
        /// <param name="invoiceId">WM invoice number.</param>
        /// <param name="culture">Response language.</param>
        public ExpressPaymentConfirmation(Purse storePurse, long invoiceId, CultureInfo culture = null)
            : this(storePurse, null, invoiceId, culture)
        {
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(
                CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Initializer.Id, StorePurse, InvoiceId, ConfirmationCode);
        }

        protected override void BuildXmlHead(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder) throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartDocument();

            xmlRequestBuilder.WriteStartElement("merchant.request"); // <merchant.request>

            xmlRequestBuilder.WriteElement("wmid", Initializer.Id.ToString());

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
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder) throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteElement("lmi_payee_purse", StorePurse.ToString());
            xmlRequestBuilder.WriteElement(
                "lmi_clientnumber_code", ConfirmationCode);
            xmlRequestBuilder.WriteElement("lmi_wminvoiceid", InvoiceId);
            xmlRequestBuilder.WriteElement("lang", Culture.Name);
        }

        protected override void BuildXmlFoot(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder) throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteEndElement(); // </merchant.request>

            xmlRequestBuilder.WriteEndDocument();
        }
    }
}
