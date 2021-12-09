using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X8. Retrieving information about purse ownership. Searching for system user by identifier or purse.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class WmIdReport : WmResponse
    {
        /// <summary>
        ///  Is the search is successful.
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// WM identifier which is being searched for.
        /// </summary>
        public WmId? WmId { get; protected set; }

        /// <summary>
        ///  If True, means that ALL incoming operations (direct payments, invoice payments, merchant.webmoney payments, X2 interface payments) are forbidden for ALL purses for the WM identifier that has been searched for. 
        /// </summary>
        public bool? TransferRejected { get; protected set; }

        /// <summary>
        /// Decimal representation of whether the user for the WM identifier that is being searched for has allowed or forbidden the acceptance of payments, messages and invoices from NONcorrespondents.
        /// Payments are indicated by the fourth bit from the right, so the decimal value 0 (binary 0000) means that the user has not set any restrictions, while the value of 8 (binary 1000) means that the user has forbidden incoming payments to the user's purses from NONcorrespondents.
        /// </summary>
        public PartnerAvailability? Availability { get; protected set; }


        /// <summary>
        /// Passport type for the WM identifier being searched for from the X11 interface.
        /// </summary>
        public PassportDegree? Passport { get; protected set; }

        /// <summary>
        /// The purse searched for.
        /// </summary>
        public Purse? Purse { get; protected set; }

        /// <summary>
        /// True means that for the WM purse in question, payment acceptance through merchant.webmoney has been enabled.
        /// If the WM identifer also has a ban on incoming payments from NONcorresopndents in the themselfcorrstate attribute, then making direct payments (including through X2) to the purse is forbidden, and payments to this purse may be made only if the WM identifer has issued an invoice or through merchant.webmoney.
        /// </summary>
        public bool? MerchantEnabled { get; protected set; }

        /// <summary>
        /// True means that for the purse in question, payment acceptance through cash terminals to merchant.webmoney is enabled, and making direct payments to the purse (including through the X2 interface) is forbidden, and payments may be made to the purse only if the WMID has issued an invoice or through merchant.webmoney.
        /// </summary>
        public bool? CashierEnabled { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Success = wmXmlPackage.SelectBool("retval");

            if (!Success)
                return;

            WmId = wmXmlPackage.SelectWmIdIfExists("testwmpurse/wmid");

            var transferRejectedValue = wmXmlPackage.SelectInt32("testwmpurse/wmid/@available");

            if (transferRejectedValue >= 0)
                TransferRejected = 1 == transferRejectedValue;

            var partnerAvailabilityValue = wmXmlPackage.SelectInt32("testwmpurse/wmid/@themselfcorrstate");

            if (partnerAvailabilityValue > 0)
                Availability = (PartnerAvailability)partnerAvailabilityValue;

            var passportValue = wmXmlPackage.SelectInt32("testwmpurse/wmid/@newattst");

            if (passportValue >= 0)
                Passport = (PassportDegree)passportValue;

            Purse = wmXmlPackage.SelectPurseIfExists("testwmpurse/purse");

            var merchantEnabledValue = wmXmlPackage.SelectInt32("testwmpurse/purse/@merchant_active_mode");

            if (merchantEnabledValue >= 0)
                MerchantEnabled = 1 == merchantEnabledValue;

            var cashierEnabled = wmXmlPackage.SelectInt32("testwmpurse/purse/@merchant_allow_cashier");

            if (cashierEnabled >= 0)
                CashierEnabled = 1 == cashierEnabled;
        }
    }
}