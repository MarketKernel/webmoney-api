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
    public class PurseInfo
    {
        /// <summary>
        /// WM purse unique internal number.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// WM purse number.
        /// </summary>
        public Purse Purse { get; protected set; }

        /// <summary>
        /// Amount on WM purse.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// Short description of the purse.
        /// </summary>
        public Description Description { get; protected set; }

        public bool Enable { get; protected set; }

        public long LastIncomingTransferId { get; protected set; }

        public long LastOutgoingTransferId { get; protected set; }

        internal void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = wmXmlPackage.SelectInt32("@id");
            Purse = wmXmlPackage.SelectPurse("pursename");
            Amount = wmXmlPackage.SelectAmount("amount");
            Description = (Description) wmXmlPackage.SelectString("desc");

            // Элементы outsideopen, lastintr, lastouttr отсутствуют при авторизации ключами Keeper Light.
            Enable = wmXmlPackage.SelectBoolIfExists("outsideopen") ?? false;
            LastIncomingTransferId = wmXmlPackage.SelectInt64IfExists("lastintr") ?? 0;
            LastOutgoingTransferId = wmXmlPackage.SelectInt64IfExists("lastouttr") ?? 0;
        }
    }
}