using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X15. Viewing and changing settings of "by trust" management.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalTrust : WmRequest<RecentTrust>
    {
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLTrustSave2.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLTrustSave2Cert.asp";

        /// <summary>
        /// Allow (True) or not (False) the identifier in the masterwmid tag to make out accounts to the trusted purse, which belongs to the slavewmid.
        /// </summary>
        public bool InvoiceAllowed { get; set; }

        /// <summary>
        ///  Allow (True) or not (False) the identifier in the masterwmid tag to transfer funds on trust from the trusted purse, which belongs to slavewmid.
        /// </summary>
        public bool TransferAllowed { get; set; }

        /// <summary>
        /// Allow (True) or not (False) the identifier in the masterwmid tag to view balance for the trusted purse, which belongs to slavewmid.
        /// </summary>
        public bool BalanceAllowed { get; set; }

        /// <summary>
        /// Allow (True) or not (False) the identifier in the masterwmid tag to view history of transactions for the purse, which belongs to slavewmid.
        /// </summary>
        public bool HistoryAllowed { get; set; }

        /// <summary>
        /// WM-identifier whom slavewmid allows/disallows (depending on the attributes in the trust tag) by this request to control his/her slavepurse.
        /// </summary>
        public WmId Master { get; set; }

        /// <summary>
        /// Purse number, which belongs to slavewmid for which trust setting are applied, the essence of which is defined by the trust tag attributes.
        /// </summary>
        public Purse Purse { get; set; }

        /// <summary>
        /// Daily limit. Money limit which master identifier is allowed to transfer from the purse during a day. For example, for the purse and master identifier daily limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer not earlier than 25 January 2007 23:59.
        /// </summary>
        public Amount Limit { get; set; }

        /// <summary>
        /// Day limit. Money limit which the specified master identifier is allowed to transfer from the purse during the current day. For example, for the purse and master identifier daily limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only after 25 January 2007 00:00, as 25 January is next day different from completion date of the previous transaction.
        /// </summary>
        public Amount DayLimit { get; set; }

        /// <summary>
        /// Weekly limit. Money limit which the specified master identifier is allowed to transfer during the current week. For example, for the purse and master identifier weekly limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only on 28 January 2007 at 00:00, as 28th January is first day of the next week different from the week when the previous transaction was completed. Keep in mind that a week starts in British (American) manner on Sunday not Monday and ends on Saturday.
        /// </summary>
        public Amount WeekLimit { get; set; }

        /// <summary>
        /// Monthly limit. Money limit which the specified master identifier is allowed to transfer during the current month. For example, for the purse and master identifier monthly limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only on 1 February 2007 at 00:00, as 1st February is the first day of the next month different from the month when the previous transaction was completed.
        /// </summary>
        public Amount MonthLimit { get; set; }

        protected internal OriginalTrust()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="master">WM-identifier whom slavewmid allows/disallows (depending on the attributes in the trust tag) by this request to control his/her slavepurse.</param>
        /// <param name="purse">Purse number, which belongs to slavewmid for which trust setting are applied, the essence of which is defined by the trust tag attributes.</param>
        public OriginalTrust(WmId master, Purse purse)
        {
            Master = master;
            Purse = purse;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Initializer.Id, Purse, Master,
                requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("trust"); // <trust>

            xmlRequestBuilder.AppendAttribute("inv", InvoiceAllowed ? 1 : 0);
            xmlRequestBuilder.AppendAttribute("trans", TransferAllowed ? 1 : 0);
            xmlRequestBuilder.AppendAttribute("purse", BalanceAllowed ? 1 : 0);
            xmlRequestBuilder.AppendAttribute("transhist", HistoryAllowed ? 1 : 0);

            xmlRequestBuilder.WriteElement("masterwmid", Master.ToString());
            xmlRequestBuilder.WriteElement("slavewmid", Initializer.Id.ToString());
            xmlRequestBuilder.WriteElement("purse", Purse.ToString());

            xmlRequestBuilder.WriteElement("limit", Limit.ToString());
            xmlRequestBuilder.WriteElement("daylimit", DayLimit.ToString());
            xmlRequestBuilder.WriteElement("weeklimit", WeekLimit.ToString());
            xmlRequestBuilder.WriteElement("monthlimit", MonthLimit.ToString());

            xmlRequestBuilder.WriteEndElement(); // </trust>
        }
    }
}