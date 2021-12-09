using System;
using System.Globalization;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X13. Recalling incomplete protected transaction.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class ProtectionRejector : WmRequest<ProtectionReport>
    {
        private long _transferId;
        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLRejectProtect.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLRejectProtectCert.asp";

        /// <summary>
        /// The tag contains transaction number (an integer, positive number) according to the WebMoney Transfer (wmtranid) internal accounting, and the transaction must be protected (by code or time), and the status of protected transaction as incomplete.
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

        protected internal ProtectionRejector()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId">The tag contains transaction number (an integer, positive number) according to the WebMoney Transfer (wmtranid) internal accounting.</param>
        public ProtectionRejector(long transferId)
        {
            if (transferId < 0)
                throw new ArgumentOutOfRangeException(nameof(transferId));

            TransferId = transferId;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", TransferId, requestNumber);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("rejectprotect"); // <rejectprotect>

            xmlRequestBuilder.WriteElement("wmtranid", TransferId);

            xmlRequestBuilder.WriteEndElement(); // </rejectprotect>
        }
    }
}