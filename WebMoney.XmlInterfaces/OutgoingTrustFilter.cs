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
    public class OutgoingTrustFilter : WmRequest<TrustRegister>
    {
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLTrustList.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLTrustListCert.asp";

        /// <summary>
        /// WM-identifier, for which trust list must be retrieved.
        /// </summary>
        public WmId WmId { get; set; }

        protected internal OutgoingTrustFilter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">WM-identifier, for which trust list must be retrieved.</param>
        public OutgoingTrustFilter(WmId wmId)
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

            xmlRequestBuilder.WriteStartElement("gettrustlist"); // <gettrustlist>

            xmlRequestBuilder.WriteElement("wmid", WmId.ToString());

            xmlRequestBuilder.WriteEndElement(); // </gettrustlist>
        }
    }
}