using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X22. Receiving the ticket of prerequest payment form at merchant.webmoney
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "merchant.response")]
    public class MerchantPaymentToken : WmResponse
    {
        /// <summary>
        /// Operation token. Transaction token which should be obligatorily passed in the link to the payment request form in the 'gid' parameter as follows: to call in Russian https://merchant.webmoney.ru/lmi/payment.asp?gid=token and in English https://merchant.wmtransfer.com/lmi/payment.asp?gid=token.
        /// </summary>
        public string Token { get; protected set; }

        /// <summary>
        /// Ticket validity period. If the value of validityperiodinhours passed in the request is less than 0, greater than 744, isn't a digit or is omitted at all, than the default value, which is '744', will be assigned as the ticket validity period.
        /// </summary>
        public ushort Lifetime { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new OriginalMerchantPaymentException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage) throw new ArgumentNullException(nameof(wmXmlPackage));

            Token = wmXmlPackage.SelectNotEmptyString("transtoken");
            Lifetime = wmXmlPackage.SelectUInt16("validityperiodinhours");
        }
    }
}
