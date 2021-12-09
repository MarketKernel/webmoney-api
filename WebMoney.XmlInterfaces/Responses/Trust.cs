using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class Trust
    {
        /// <summary>
        /// Unique trust number in WebMoney system.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Allowed (True) or not (False) in the master tag copying accounts to the trusted purse, which belongs to the gettrustlist\wmid.
        /// </summary>
        public bool InvoiceAllowed { get; protected set; }

        /// <summary>
        /// Allowed (True) or not (False) specified in the master tag to transfer funds on trust from a trusted purse, which belongs to the gettrustlist\wmid.
        /// </summary>
        public bool TransferAllowed { get; protected set; }

        /// <summary>
        /// Allowed (True) or not (False) in the master tag to view balance on trusted purse, which belongs to gettrustlist\wmid.
        /// </summary>
        public bool BalanceAllowed { get; protected set; }

        /// <summary>
        /// Allowed (True) or not (False) in the master tag to view history of transfers of the purse, which belongs to the gettrustlist\wmid.
        /// </summary>
        public bool HistoryAllowed { get; protected set; }

        /// <summary>
        /// The identifier is trusted to make any actions with the purse.
        /// </summary>
        public WmId Master { get; protected set; }

        /// <summary>
        /// Purse, which belongs to the gettrustlist\wmid over whom master identifier is allowed to take any actions.
        /// </summary>
        public Purse? Purse { get; protected set; }

        /// <summary>
        /// Daily limit. Money limit which master identifier is allowed to transfer from the purse during a day. For example, for the purse and master identifier daily limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer not earlier than 25 January 2007 23:59.
        /// </summary>
        public Amount Hours24Limit { get; protected set; }

        /// <summary>
        /// Day limit. Money limit which the specified master identifier is allowed to transfer from the purse during the current day. For example, for the purse and master identifier daily limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only after 25 January 2007 00:00, as 25 January is next day different from completion date of the previous transaction.
        /// </summary>
        public Amount DayLimit { get; protected set; }

        /// <summary>
        /// Weekly limit. Money limit which the specified master identifier is allowed to transfer during the current week. For example, for the purse and master identifier weekly limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only on 28 January 2007 at 00:00, as 28th January is first day of the next week different from the week when the previous transaction was completed. Keep in mind that a week starts in British (American) manner on Sunday not Monday and ends on Saturday.
        /// </summary>
        public Amount WeekLimit { get; protected set; }

        /// <summary>
        /// Monthly limit. Money limit which the specified master identifier is allowed to transfer during the current month. For example, for the purse and master identifier monthly limit is set to 1 WMR, master identifier on 24 January 2007 at 23:59 made a transfer on trust from the purse equal to 1 WMR. It means that next time the master identifier can make equal transfer only on 1 February 2007 at 00:00, as 1st February is the first day of the next month different from the month when the previous transaction was completed.
        /// </summary>
        public Amount MonthLimit { get; protected set; }

        /// <summary>
        /// Daily sum. Total sum of already completed transactions by master indentifier from purse for the same day, month and year specified in comletion date of the previous transaction on trust - lastsumdate.
        /// </summary>
        public Amount DayAmount { get; protected set; }

        /// <summary>
        /// Weekly sum. Total sum of already completed transactions by master indentifier from purse for the same week and year, specified in comletion date of the previous transaction on trust - lastsumdate.
        /// </summary>
        public Amount WeekAmount { get; protected set; }

        /// <summary>
        /// Monthly sum. Total sum of already completed transactions by master indentifier from purse for the same month and year, specified in comletion date of the previous transaction on trust - lastsumdate.
        /// </summary>
        public Amount MonthAmount { get; protected set; }

        /// <summary>
        /// Date of the last transcaction. The date when master identifier made the last transaction on trust from purse.
        /// </summary>
        public WmDate LastDate { get; protected set; }

        internal void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = wmXmlPackage.SelectInt32("@id");
            InvoiceAllowed = wmXmlPackage.SelectBool("@inv");
            TransferAllowed = wmXmlPackage.SelectBool("@trans");
            BalanceAllowed = wmXmlPackage.SelectBool("@purse");
            HistoryAllowed = wmXmlPackage.SelectBool("@transhist");
            Master = wmXmlPackage.SelectWmId("master");
            Purse = wmXmlPackage.SelectPurseIfExists("purse");
            Hours24Limit = wmXmlPackage.SelectAmount("daylimit");
            DayLimit = wmXmlPackage.SelectAmount("dlimit");
            WeekLimit = wmXmlPackage.SelectAmount("wlimit");
            MonthLimit = wmXmlPackage.SelectAmount("mlimit");
            DayAmount = wmXmlPackage.SelectAmount("dsum");
            WeekAmount = wmXmlPackage.SelectAmount("wsum");
            MonthAmount = wmXmlPackage.SelectAmount("msum");
            LastDate = wmXmlPackage.SelectWmDate("lastsumdate");
        }
    }
}