using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X8. Retrieving information about purse ownership. Searching for system user by identifier or purse.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class WmIdFinder : WmRequest<WmIdReport>
    {
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLFindWMPurseNew.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLFindWMPurseCertNew.asp";

        /// <summary>
        /// WM identifier to search.
        /// </summary>
        public WmId? WmId { get; private set; }

        /// <summary>
        /// Purse to search.
        /// </summary>
        public Purse? Purse { get; private set; }

        protected internal WmIdFinder()
        {
        }

        /// <summary>
        /// WM identifier to search.
        /// </summary>
        /// <param name="wmId"></param>
        public WmIdFinder(WmId wmId)
        {
            WmId = wmId;
        }

        /// <summary>
        /// Purse to search.
        /// </summary>
        /// <param name="purse"></param>
        public WmIdFinder(Purse purse)
        {
            Purse = purse;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", WmId, Purse);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("testwmpurse"); // <testwmpurse>

            if (WmId.HasValue)
                xmlRequestBuilder.WriteElement("wmid", WmId.ToString());

            if (Purse.HasValue)
                xmlRequestBuilder.WriteElement("purse", Purse.ToString());

            xmlRequestBuilder.WriteEndElement(); // </testwmpurse>
        }
    }
}