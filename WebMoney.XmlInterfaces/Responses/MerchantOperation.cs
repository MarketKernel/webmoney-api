using System;
using System.Globalization;
using System.Net;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X18. Getting transaction details via merchant.wmtransfer.com.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "merchant.response")]
    public class MerchantOperation : WmResponse
    {
        /// <summary>
        /// Unique transaction number in WebMoney Transfer.
        /// </summary>
        public long TransferId { get; protected set; }

        /// <summary>
        /// Unique invoice number in WebMoney Transfer.
        /// </summary>
        public long InvoiceId { get; protected set; }

        /// <summary>
        /// The amount of WebMoney units transferred in this transaction to the merchant.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// Server date of the transaction in WebMoney Transfer.
        /// </summary>
        public WmDateTime CreateTime { get; protected set; }

        /// <summary>
        /// The payment purpose as submitted to the merchant.wmtransfer.com service in the lmi_payment_desc field.
        /// </summary>
        public Description Description { get; protected set; }

        /// <summary>
        /// WMID to which pursefrom belongs.
        /// </summary>
        public WmId SourceWmId { get; protected set; }

        /// <summary>
        /// Purchaser's purse. Note that when paying via terminals or ATMs, or Paymer check or WM card the purse of the corresponding gateway or Paymer service will be passed as Payer's purse.
        /// </summary>
        public Purse SourcePurse { get; protected set; }

        /// <summary>
        /// If the flag is set to True - it means that the payer used the purse of the Capitaller service and not his/her own purse for the payment; if set to False - the payer used his/her own purse for the payment.
        /// </summary>
        public bool CapitallerFlag { get; protected set; }

        /// <summary>
        /// If this flag is set to 1 - it means that the payment was made by a user who authorized via the E-num.ru service.
        /// </summary>
        public byte EnumFlag { get; protected set; }

        /// <summary>
        /// The IP address of the user who made the payment.
        /// </summary>
        public IPAddress IpAddress { get; protected set; }

        /// <summary>
        /// The phone number of the payer, if the payment was made via WM Keeper Mobile.
        /// </summary>
        public string TelepatPhone { get; protected set; }

        /// <summary>
        /// 0 value returned here indicates that the payment had been made at merchant.wmtransfer site via Keeper Mobile; 1 - the payment had been made via SMS at the merchant's site by means of X20 interface; 2 - at the Merchant WebMoney site in the mobile payment section with the owner's WMID found by mobile phone number and SMS-confirmation.
        /// </summary>
        public TelepatMethod? TelepatMethod { get; protected set; }

        /// <summary>
        /// The number of the check or payer's WM card if the payment was made by means of Paymer check or WM card.
        /// </summary>
        public string PaymerNumber { get; protected set; }

        /// <summary>
        /// Payer's e-mail address, which he/she specified when paying by means of Paymer check, WM card or WM- Note for WebMoney Check this field is kept empty.
        /// </summary>
        public string PaymerEmail { get; protected set; }

        /// <summary>
        /// 0 if the payment had been made with a Paymer check or a WM-card; '1' - if the payment had been made with a WM-note '2' - if the payment had been made via WebMoney Check service
        /// </summary>
        public PaymerType PaymerType { get; protected set; }

        /// <summary>
        /// The number of the payment, if payer paid via a terminal, ATM or post office. Note, that this parameter is reserved for compatibility, and for the moment payments made via terminals, cash points and checkout counters are performed by means of WebMoney Check service. See paymer_type - parameter above.
        /// </summary>
        public string CashierNumber { get; protected set; }

        /// <summary>
        /// The date of the payment, if it was made via a terminal, ATM or post office. Note, that this parameter is reserved for compatibility, and for the moment payments made via terminals, cash points and checkout counters are performed by means of WebMoney Check service. See paymer_type - parameter above.
        /// </summary>
        public WmDateTime? CashierDate { get; protected set; }

        /// <summary>
        /// The amount of the payment, if it was made via a terminal, ATM or post office. Note, that this parameter is reserved for compatibility, and for the moment payments made via terminals, cash points and checkout counters are performed by means of WebMoney Check service. See paymer_type - parameter above.
        /// </summary>
        public Amount? CashierAmount { get; protected set; }

        /// <summary>
        /// If this parameter is available, that means the payment is to be performed by a method which doesn't require a registration in the System; 0 - for money transfer systems, 3 - for Alpha-click, 4 - for Russian banks cards, 5 - for Russian Standart bank internet-banking system, 6 - for VTB24 internet-banking system, 7 - for THANK YOU from Sberbank bonus point, 8 - for payment terminals and banks (for U-purses only).
        /// </summary>
        public int? SdpType { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            var wmXmlPackage = (WmXmlPackage)xmlPackage;

            int errorNumber = wmXmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
            {
                MerchantOperationObtainerException.ErrorExtendedInfo errorExtendedInfo = null;

                if (wmXmlPackage.Exists("errorlog/err_code"))
                {
                    string extendedErrorNumberValue = wmXmlPackage.SelectString("errorlog/err_code");

                    if (!string.IsNullOrEmpty(extendedErrorNumberValue))
                    {
                        // TODO [L] Расшифровать errorlog/siteid
                        // TODO [L] Расшифровать errorlog/att
                        errorExtendedInfo = new MerchantOperationObtainerException.ErrorExtendedInfo
                        {
                            ExtendedErrorNumber = int.Parse(extendedErrorNumberValue,
                                CultureInfo.InvariantCulture.NumberFormat),
                            StorePurse = wmXmlPackage.SelectPurse("errorlog/@lmi_payee_purse"),
                            OrderId = wmXmlPackage.SelectInt32("errorlog/@lmi_payment_no"),
                            PaymentInfoCreateTime = wmXmlPackage.SelectWmDateTime("errorlog/datecrt"),
                            PaymentInfoUpdateTime = wmXmlPackage.SelectWmDateTime("errorlog/dateupd"),
                            EnterTime = wmXmlPackage.SelectWmDateTimeIfExists("errorlog/date_s"),
                            AuthorizationTime = wmXmlPackage.SelectWmDateTimeIfExists("errorlog/date_pc"),
                            ConfirmationTime = wmXmlPackage.SelectWmDateTimeIfExists("errorlog/date_pd"),
                            SiteId = wmXmlPackage.SelectInt32("errorlog/siteid"),
                            PaymentMethod = wmXmlPackage.SelectString("errorlog/att")
                        };
                    }
                }

                throw new MerchantOperationObtainerException(errorNumber, xmlPackage.SelectString("retdesc"))
                {
                    ExtendedInfo = errorExtendedInfo
                };
            }
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            TransferId = wmXmlPackage.SelectInt64("operation/@wmtransid");
            InvoiceId = wmXmlPackage.SelectInt64("operation/@wminvoiceid");
            CreateTime = wmXmlPackage.SelectWmDateTime("operation/operdate");
            Description = (Description)wmXmlPackage.SelectString("operation/purpose");
            SourcePurse = wmXmlPackage.SelectPurse("operation/pursefrom");
            SourceWmId = wmXmlPackage.SelectWmId("operation/wmidfrom");
            CapitallerFlag = wmXmlPackage.SelectBoolIfExists("operation/capitallerflag") ?? false;
            EnumFlag = wmXmlPackage.SelectUInt8IfExists("operation/enumflag") ?? 0;
            IpAddress = IPAddress.Parse(wmXmlPackage.SelectNotEmptyString("operation/IPAddress"));
            TelepatPhone = wmXmlPackage.SelectString("operation/telepat_phone");
            TelepatMethod =
                (TelepatMethod?) wmXmlPackage.SelectEnumFromIntegerIfExists(typeof(TelepatMethod),
                    "operation/telepat_paytype");
            PaymerNumber = wmXmlPackage.TrySelectNotEmptyString("operation/paymer_number");
            PaymerEmail = wmXmlPackage.TrySelectNotEmptyString("operation/paymer_email");
            PaymerType =
                (PaymerType?) wmXmlPackage.SelectEnumFromIntegerIfExists(typeof(PaymerType),
                    "operation/paymer_type") ?? PaymerType.None;

            //CashierNumber = wmXmlResponsePackage.SelectString("operation/cashier_number");

            //if (!string.IsNullOrEmpty(wmXmlResponsePackage.SelectString("operation/cashier_date")))
            //    CashierDate = wmXmlResponsePackage.SelectWmDateTime("operation/cashier_date");

            //if (!string.IsNullOrEmpty(wmXmlResponsePackage.SelectString("operation/cashier_amount")))
            //    CashierAmount = wmXmlResponsePackage.SelectAmount("operation/cashier_amount");

            SdpType = wmXmlPackage.SelectInt32IfExists("operation/sdp_type");
        }
    }
}