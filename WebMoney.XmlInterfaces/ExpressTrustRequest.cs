using System;
using System.Globalization;
using System.Net.Mail;
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
    public class ExpressTrustRequest : WmRequest<ExpressTrustResponse>
    {
        protected override string ClassicUrl => "https://merchant.webmoney.ru/conf/xml/XMLTrustRequest.asp";

        protected override string LightUrl => "https://merchant.wmtransfer.com/conf/xml/XMLTrustRequest.asp";

        /// <summary>
        /// WM purse of payment recipient. Number of the purse through which the merchant will accept buyer payments with the help of the Х2 interface.
        /// </summary>
        public Purse StorePurse { get; set; }

        /// <summary>
        /// Daily limit. Maximum amount that the merchant can transfer from the buyer in one calendar day, in the same WM currency as the purse of the merchant lmi_payee_purse.
        /// If the amount is zero, then there is no limit on payments per day. At least one of the three limits (daily, weekly, monthly) must be given a value.
        /// It is not possible for there to be no limits at all, or that is to say, for all three limits to equal zero.
        /// The maximum amount which can be set through this interface is the same as in WebMoney Keeper Standard financial restrictions for Keeper Standard with SMS or ENUM-confirmation of transactions enabled.
        /// If for some reason a store or service needs a larger amount, then tell the buyer to raise the limit independently in their trust settings, at security.wmtransfer.com.
        /// </summary>
        public Amount DayLimit { get; set; }

        /// <summary>
        /// Weekly limit. Analogous to lmi_day_limit (substituting "calendar week").
        /// </summary>
        public Amount WeekLimit { get; set; }

        /// <summary>
        /// Monthly limit. Analogous to lmi_day_limit (substituting "calendar month").
        /// </summary>
        public Amount MonthLimit { get; set; }

        /// <summary>
        /// Cell phone, with country code and city/area code (numbers only, without plus signs, parentheses and other symbols). Examples: Russia – 79167777777, Ukraine – 380527777777, U.S. – 12127777777).
        /// </summary>
        public Phone ClientPhone { get; set; }

        /// <summary>
        /// Buyer's WMID (12 numbers exactly).
        /// </summary>
        public WmId ClientWmId { get; set; }

        /// <summary>
        /// Buyer's WM purse (upper-case letter and 12 numbers).
        /// </summary>
        public Purse ClientPurse { get; set; }

        /// <summary>
        /// Buyer's e-mail address. The interface will automatically find the WMID from which buyer payment can be made.
        /// </summary>
        public MailAddress ClientEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClientIdType ClientIdType { get; }

        /// <summary>
        /// Type of data passed in lmi_clientnumber.
        /// </summary>
        public ConfirmationType ConfirmationType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private CultureInfo _culture;

        /// <summary>
        /// Response language. This parameter passes the values ru-RU and en-US for the Russian- and English-language interfaces, respectively. This value defines the language used in the SMS messages (USSD queries) sent to the user and the language of the responses in the userdesc tag.
        /// </summary>
        public CultureInfo Culture
        {
            get => _culture;
            set => _culture = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected internal ExpressTrustRequest()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of payment recipient.</param>
        /// <param name="dayLimit">Daily limit.</param>
        /// <param name="weekLimit">Weekly limit.</param>
        /// <param name="monthLimit">Monthly limit.</param>
        /// <param name="clientPhone">Cell phone, with country code and city/area code.</param>
        /// <param name="confirmationType">Type of data passed in lmi_clientnumber.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustRequest(
            Purse storePurse,
            Amount dayLimit,
            Amount weekLimit,
            Amount monthLimit,
            Phone clientPhone,
            ConfirmationType confirmationType,
            CultureInfo culture = null)
        {
            if (null == culture)
                culture = Thread.CurrentThread.CurrentUICulture;

            StorePurse = storePurse;
            DayLimit = dayLimit;
            WeekLimit = weekLimit;
            MonthLimit = monthLimit;
            ClientPhone = clientPhone;
            ClientIdType = ClientIdType.Phone;
            ConfirmationType = confirmationType;
            Culture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of payment recipient.</param>
        /// <param name="dayLimit">Daily limit.</param>
        /// <param name="weekLimit">Weekly limit.</param>
        /// <param name="monthLimit">Monthly limit.</param>
        /// <param name="clientWmId">Buyer's WMID (12 numbers exactly).</param>
        /// <param name="confirmationType">Type of data passed in lmi_clientnumber.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustRequest(
            Purse storePurse,
            Amount dayLimit,
            Amount weekLimit,
            Amount monthLimit,
            WmId clientWmId,
            ConfirmationType confirmationType,
            CultureInfo culture = null)
        {
            if (null == culture)
                culture = Thread.CurrentThread.CurrentUICulture;

            StorePurse = storePurse;
            DayLimit = dayLimit;
            WeekLimit = weekLimit;
            MonthLimit = monthLimit;
            ClientWmId = clientWmId;
            ClientIdType = ClientIdType.WmId;
            ConfirmationType = confirmationType;
            Culture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of payment recipient.</param>
        /// <param name="dayLimit">Daily limit.</param>
        /// <param name="weekLimit">Weekly limit.</param>
        /// <param name="monthLimit">Monthly limit.</param>
        /// <param name="clientPurse">Buyer's WM purse (upper-case letter and 12 numbers).</param>
        /// <param name="confirmationType">Type of data passed in lmi_clientnumber.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustRequest(
            Purse storePurse,
            Amount dayLimit,
            Amount weekLimit,
            Amount monthLimit,
            Purse clientPurse,
            ConfirmationType confirmationType,
            CultureInfo culture = null)
        {
            if (null == culture)
                culture = Thread.CurrentThread.CurrentUICulture;

            StorePurse = storePurse;
            DayLimit = dayLimit;
            WeekLimit = weekLimit;
            MonthLimit = monthLimit;
            ClientPurse = clientPurse;
            ClientIdType = ClientIdType.WmId;
            ConfirmationType = confirmationType;
            Culture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePurse">WM purse of payment recipient.</param>
        /// <param name="dayLimit">Daily limit.</param>
        /// <param name="weekLimit">Weekly limit.</param>
        /// <param name="monthLimit">Monthly limit.</param>
        /// <param name="clientEmail">Buyer's e-mail address.</param>
        /// <param name="confirmationType">Type of data passed in lmi_clientnumber.</param>
        /// <param name="culture">Response language.</param>
        public ExpressTrustRequest(
            Purse storePurse,
            Amount dayLimit,
            Amount weekLimit,
            Amount monthLimit,
            MailAddress clientEmail,
            ConfirmationType confirmationType,
            CultureInfo culture = null)
        {
            if (null == clientEmail)
                throw new ArgumentNullException(nameof(clientEmail));

            if (null == culture)
                culture = Thread.CurrentThread.CurrentCulture;

            StorePurse = storePurse;
            DayLimit = dayLimit;
            WeekLimit = weekLimit;
            MonthLimit = monthLimit;
            ClientEmail = clientEmail;
            ClientIdType = ClientIdType.Email;
            ConfirmationType = confirmationType;
            Culture = culture;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}{2}{3}{4}",
                Initializer.Id,
                StorePurse,
                GetClientId(),
                (int)ClientIdType,
                (int)ConfirmationType);
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

            xmlRequestBuilder.WriteElement("lmi_payee_purse", StorePurse.ToString());
            xmlRequestBuilder.WriteElement("lmi_day_limit", DayLimit.ToString());
            xmlRequestBuilder.WriteElement("lmi_week_limit", WeekLimit.ToString());
            xmlRequestBuilder.WriteElement("lmi_month_limit", MonthLimit.ToString());
            xmlRequestBuilder.WriteElement("lmi_clientnumber", GetClientId());
            xmlRequestBuilder.WriteElement("lmi_clientnumber_type", ((int)ClientIdType).ToString(CultureInfo.InvariantCulture));
            xmlRequestBuilder.WriteElement("lmi_sms_type", ((int)ConfirmationType).ToString(CultureInfo.InvariantCulture));
            xmlRequestBuilder.WriteElement("lang", Culture.Name);
        }

        protected override void BuildXmlFoot(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteEndElement(); // </merchant.request>

            xmlRequestBuilder.WriteEndDocument();
        }

        private string GetClientId()
        {
            string clientId;

            switch (ClientIdType)
            {
                case ClientIdType.Phone:
                    clientId = ClientPhone.ToString().Remove(0, 1);
                    break;
                case ClientIdType.WmId:
                    clientId = ClientWmId.ToString();
                    break;
                case ClientIdType.Purse:
                    clientId = ClientPurse.ToString();
                    break;
                case ClientIdType.Email:
                    clientId = ClientEmail.Address;
                    break;
                default:
                    throw new InvalidOperationException("ClientIdType=" + ClientIdType);
            }

            return clientId;
        }
    }
}
