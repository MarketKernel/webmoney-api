using System;
using System.Globalization;
using System.Threading;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X21. Setting trust for merchant payments by SMS.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class ExpressTrustConfirmation : WmRequest<ExpressTrustReport>
    {
        private CultureInfo _culture;
        private int _reference;

        protected override string ClassicUrl => "https://merchant.webmoney.ru/conf/xml/XMLTrustConfirm.asp";

        protected override string LightUrl => "https://merchant.wmtransfer.com/conf/xml/XMLTrustConfirm.asp";

        /// <summary>
        /// WM number of the query. Number of the query from the response during the previous call.
        /// </summary>
        public int Reference
        {
            get => _reference;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _reference = value;
            }
        }

        /// <summary>
        /// Buyer code. This field passes the digital code which the buyer received via mobile phone for confirming payment.
        /// If no SMS was sent to the buyer (a USSD query was sent), then you should pass a code with a value of 0 here.
        /// </summary>
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// Response language. This parameter passes the values ru-RU and en-US for the Russian- and English-language interfaces, respectively. This value defines the language used in the SMS messages (USSD queries) sent to the user and the language of the responses in the userdesc tag.
        /// </summary>
        public CultureInfo Culture
        {
            get => _culture;
            set => _culture = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected internal ExpressTrustConfirmation()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"> WM number of the query.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustConfirmation(int reference, CultureInfo culture = null)
            :this(reference, null, culture)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"> WM number of the query.</param>
        /// <param name="confirmationCode">Buyer code. This field passes the digital code which the buyer received via mobile phone for confirming payment.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustConfirmation(int reference, string confirmationCode, CultureInfo culture = null)
        {
            if (null == culture)
                culture = Thread.CurrentThread.CurrentUICulture;

            if (reference < 0)
                throw new ArgumentOutOfRangeException(nameof(reference));

            Reference = reference;
            ConfirmationCode = confirmationCode??"0";
            Culture = culture;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Initializer.Id, Reference, ConfirmationCode);
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

            xmlRequestBuilder.WriteElement("lmi_purseid", Reference);
            xmlRequestBuilder.WriteElement("lmi_clientnumber_code", ConfirmationCode);
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
