using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X5. Completing a code-protected transaction. Entering a protection code.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class ProtectionFinisher : WmRequest<ProtectionReport>
    {
        private long _transferId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLFinishProtect.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLFinishProtectCert.asp";

        /// <summary>
        /// Unique transaction number in the WebMoney system.
        /// </summary>
        public long TransferId
        {
            get => _transferId;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _transferId = value;
            }
        }

        /// <summary>
        /// Protection code.
        /// To complete the payment performed by means of Web Merchant Interface using the holding feature, the parameter should be epmty.
        /// </summary>
        public Description Code { get; set; }

        protected internal ProtectionFinisher()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId"></param>
        /// <param name="code"></param>
        public ProtectionFinisher(long transferId, Description code)
        {
            if (transferId < 0)
                throw new ArgumentOutOfRangeException(nameof(transferId));

            TransferId = transferId;
            Code = code;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", TransferId, Code, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("finishprotect"); // <finishprotect>

            xmlRequestBuilder.WriteElement("wmtranid", TransferId);
            xmlRequestBuilder.WriteElement("pcode", Code);

            xmlRequestBuilder.WriteEndElement(); // </finishprotect>
        }
    }
}