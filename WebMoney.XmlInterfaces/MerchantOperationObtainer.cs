using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X18. Getting transaction details via merchant.wmtransfer.com.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class MerchantOperationObtainer : WmRequest<MerchantOperation>
    {
        private long _number;
        protected override string ClassicUrl => "https://merchant.webmoney.ru/conf/xml/XMLTransGet.asp";

        protected override string LightUrl => throw new NotSupportedException();

        /// <summary>
        /// WM purse of payment recipient. Purse number to which lmi_payment_no payment was received and for which the status needs to be determined.
        /// </summary>
        public Purse TargetPurse { get; set; }

        /// <summary>
        /// Payment number.
        /// </summary>
        public long Number
        {
            get => _number;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _number = value;
            }
        }

        /// <summary>
        /// Payment number type.
        /// </summary>
        public PaymentNumberKind NumberType { get; set; }

        protected internal MerchantOperationObtainer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPurse">WM purse of payment recipient.</param>
        /// <param name="number">Payment number.</param>
        /// <param name="numberType">Payment number type.</param>
        public MerchantOperationObtainer(Purse targetPurse, long number, PaymentNumberKind numberType)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number));

            TargetPurse = targetPurse;
            Number = number;
            NumberType = numberType;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Initializer.Id, TargetPurse, Number);
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
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteElement("lmi_payee_purse", TargetPurse.ToString());
            xmlRequestBuilder.WriteElement("lmi_payment_no_type", (int)NumberType);
            xmlRequestBuilder.WriteElement("lmi_payment_no", Number);
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