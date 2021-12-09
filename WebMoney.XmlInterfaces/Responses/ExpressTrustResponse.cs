using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X21. Setting trust for merchant payments by SMS.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "merchant.response")]
    public class ExpressTrustResponse : WmResponse
    {
        /// <summary>
        /// WM number of the query. Number of the query in the WMT system; then passed to the second query for checking or confirming the trust limit.
        /// </summary>
        public int Reference { get; protected set; }

        /// <summary>
        /// SMS sending type. If 1 is passed in this field, an SMS was sent to the buyer. If 2 is passed, a USSD query was sent.
        /// </summary>
        public ConfirmationType ConfirmationType { get; protected set; }

        /// <summary>
        /// Info for buyer. In case of an error, this text can be displayed to the user as instructions, which help to quickly and correctly figure out what to do to avoid such errors.
        /// </summary>
        public string Info { get; protected set; }

        public string SmsReference { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new ExpressTrustException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage) throw new ArgumentNullException(nameof(wmXmlPackage));

            Reference = wmXmlPackage.SelectInt32("trust/@purseid");
            ConfirmationType = (ConfirmationType)wmXmlPackage.SelectInt32("trust/realsmstype");
            Info = wmXmlPackage.SelectString("userdesc");
            SmsReference = wmXmlPackage.SelectString("smssecureid");
        }
    }
}
