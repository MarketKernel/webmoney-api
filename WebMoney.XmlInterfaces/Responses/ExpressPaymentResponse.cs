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
    public class ExpressPaymentResponse : WmResponse
    {
        /// <summary>
        /// WM invoice number. Unique invoice number in the WMT system.
        /// </summary>
        public long InvoiceId { get; protected set; }

        /// <summary>
        /// Type of SMS sent.
        /// </summary>
        public ConfirmationType ConfirmationType { get; protected set; }

        /// <summary>
        /// Information for the client. In case of an error, this text can be sent to the client as an instruction that will help prevent this error in the future.
        /// </summary>
        public string Info { get; protected set; }

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

            InvoiceId = wmXmlPackage.SelectInt64("operation/@wminvoiceid");
            ConfirmationType = (ConfirmationType)wmXmlPackage.SelectInt32("operation/realsmstype");
            Info = wmXmlPackage.SelectString("userdesc");
        }
    }
}
