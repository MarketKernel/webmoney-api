using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X9. Retrieving information about purse balance.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class PurseInfoFilter : WmRequest<PurseInfoRegister>
    {
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLPurses.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLPursesCert.asp";

        /// <summary>
        /// WM-identifier.
        /// </summary>
        public WmId WmId { get; set; }

        protected internal PurseInfoFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">WM-identifier.</param>
        public PurseInfoFilter(WmId wmId)
        {
            WmId = wmId;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", WmId, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("getpurses"); // <getpurses>

            xmlRequestBuilder.WriteElement("wmid", WmId.ToString());

            xmlRequestBuilder.WriteEndElement(); // </getpurses>
        }
    }
}