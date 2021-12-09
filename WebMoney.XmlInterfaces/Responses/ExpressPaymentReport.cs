using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// X20 interface. Making transactions through the merchant.webmoney service without leaving the seller's site (resource, service, application).
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "merchant.response")]
    public class ExpressPaymentReport : WmResponse
    {
        /// <summary>
        /// WM transaction number. A unique transaction number. Only if this parameter contains a positive number greater than 0, it means that the transaction was successful.
        /// For regular sellers using the merchant.webmoney service, the presence of this unique transaction number means that the client's payment has been received and charged to the purse.
        /// For sellers using processing.webmoney, the presence of the wmtransid value signifies the receipt of the payment, its addition to the current registry and queueing for inclusion into the next bank wire from this registry.
        /// For sellers with a Capitaller WMID who configured it to receive payments through merchant.webmoney, it means that the payment has been received to the incoming purse and will be distributed shortly according to the policy of the automated budget tool.
        /// If the seller's application does not have a wmtransid number (it wasn't registered or saved in its own accounting systsem), no products or services can be sold and no account can be refilled.
        /// In the request is terminated due to a connection timeout or interruption, it should be re-sent until a wmtranid value or a clear retval error code value is received.
        /// </summary>
        public long TransferId { get; protected set; }

        /// <summary>
        /// Unique invoice number in the WMT system.
        /// </summary>
        public long InvoiceId { get; protected set; }

        /// <summary>
        /// Payment amount. The payment amount in the WM currency of the same type as that in the seller's purse.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// The date of the wmtransid payment (according to the system server clock).
        /// </summary>
        public WmDateTime Date { get; protected set; }

        public Description Description { get; protected set; }

        /// <summary>
        /// Client's purse. The number of the client's WM purse that was used in the transaction.
        /// If the payment was made from a WebMoney Check (the initializaion request contained a mobile phone number, the system identified a check associated with this phone number and sent a confirmation SMS message), a Paymer purse of a corresponding type (e.g. Z000000000001 (or R, G, U, B, E) ) will be used.
        /// </summary>
        public Purse ClientPurse { get; protected set; }

        /// <summary>
        /// Client's WMID. The WMID of the client that initiated the transaction. If the payment was made from a WebMoney Check, the system will use a WMID of the Paymer check system 000000000000.
        /// </summary>
        public WmId ClientWmId { get; protected set; }

        /// <summary>
        /// Information for the client. In case of an error, this text can be sent to the user as an instruction that will help avoid this error in the future.
        /// </summary>
        public string Info { get; protected set; }

        /// <summary>
        /// SMS sending status. If the interface is terminated with 553 error (when the payment is made via Webmoney Check and LMI_CLIENTNUMBER_CODE=0 is transmitted to ascertain the payment state) or with 556 error (when the payment had been made from a WM-purse and hasn't yet been completed) then the given tag is added to the response (if SMS or USSD had been sent, i.e. LMI_SMS_TYPE= 1,2,3) and contains the status of SMS ot USSD.
        /// </summary>
        public SmsState SmsState { get; private set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new ExpressPaymentException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage) throw new ArgumentNullException(nameof(wmXmlPackage));

            TransferId = wmXmlPackage.SelectInt64("operation/@wmtransid");
            InvoiceId = wmXmlPackage.SelectInt64("operation/@wminvoiceid");
            Amount = wmXmlPackage.SelectAmount("operation/amount");
            Date = wmXmlPackage.SelectWmDateTime("operation/operdate");
            Description = (Description) wmXmlPackage.SelectString("operation/purpose");
            ClientPurse = wmXmlPackage.SelectPurse("operation/pursefrom");
            ClientWmId = wmXmlPackage.SelectWmId("operation/wmidfrom");
            Info = wmXmlPackage.SelectString("userdesc");
            SmsState = (SmsState?) wmXmlPackage.SelectEnumIfExists(typeof(SmsState), "smssentstate") ??
                       SmsState.BUFFERED;
        }
    }
}