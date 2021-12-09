using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X16. Creating a purse.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalPurse : WmRequest<RecentPurse>
    {
        private Description _description;

        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLCreatePurse.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLCreatePurseCert.asp";

        /// <summary>
        /// WM-identifier, which the created purse will belong to. Actually this WM-identifier must be the same as the identifier passed in the wmid tag of the identifier signing the request, as a purse can be created only for the identifier signing the request; it is impossible to work with the interface by trust.
        /// </summary>
        public WmId WmId { get; set; }


        /// <summary>
        /// Text name of the purse, which will be displayed in the Webmoney Keeper WinPro or Light interface.
        /// </summary>
        public Description Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
                _description = value;
            }
        }

        /// <summary>
        /// Type of the created purse as one Latin character in upper case B ,C ,D ,E ,G ,R ,U ,Y ,Z.
        /// </summary>
        public WmCurrency PurseType { get; set; }

        protected internal OriginalPurse()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">WM-identifier, which the created purse will belong to. Actually this WM-identifier must be the same as the identifier passed in the wmid tag of the identifier signing the request, as a purse can be created only for the identifier signing the request; it is impossible to work with the interface by trust.</param>
        /// <param name="purseType">Type of the created purse as one Latin character in upper case B ,C ,D ,E ,G ,R ,U ,Y ,Z.</param>
        /// <param name="description">Text name of the purse, which will be displayed in the Webmoney Keeper WinPro or Light interface.</param>
        public OriginalPurse(WmId wmId, WmCurrency purseType, Description description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            WmId = wmId;
            PurseType = purseType;
            Description = description;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", WmId, Purse.CurrencyToLetter(PurseType),
                                     requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("createpurse"); // <createpurse>

            xmlRequestBuilder.WriteElement("wmid", WmId.ToString());
            xmlRequestBuilder.WriteElement("pursetype", Purse.CurrencyToLetter(PurseType).ToString());
            xmlRequestBuilder.WriteElement("desc", Description);

            xmlRequestBuilder.WriteEndElement(); // </createpurse>
        }
    }
}