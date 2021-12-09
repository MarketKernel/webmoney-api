using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X6. Sending message to random WM-identifier via internal mail.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class OriginalMessage : WmRequest<RecentMessage>
    {
        private Message _content;

        protected override string ClassicUrl => "https://w3s.webmoney.ru/asp/XMLSendMsg.asp";

        protected override string LightUrl => "https://w3s.wmtransfer.com/asp/XMLSendMsgCert.asp";

        /// <summary>
        /// WM identifier of message recepient.
        /// </summary>
        public WmId WmId { get; set; }

        /// <summary>
        /// Message subject.
        /// </summary>
        public Description Subject { get; set; }

        /// <summary>
        /// Message text.
        /// </summary>
        public Message Content
        {
            get => _content;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value));

                _content = value;
            }
        }

        /// <summary>
        /// If True - then messages are sent without considering recipient's permission for this action. If False - then messages are sent only if the recipient permission (otherwise error code 35 is returned).
        /// Users can forbid to send them messages in two cases. The first case is when the recipient forbade to send him messages for this specific correspondent. The second case is when the reciient forbade to send him messages for unauthorized correspondents, and the sender is unauthorized.
        /// </summary>
        public bool Force { get; set; }

        protected internal OriginalMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wmId">WM identifier of message recepient.</param>
        /// <param name="content">Message text.</param>
        public OriginalMessage(WmId wmId, Message content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            WmId = wmId;
            Content = content;
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", WmId, requestNumber, Content, Subject);
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("message"); // <message>

            xmlRequestBuilder.WriteElement("receiverwmid", WmId.ToString());
            xmlRequestBuilder.WriteElement("msgsubj", Subject);
            xmlRequestBuilder.WriteElement("msgtext", Content);
            xmlRequestBuilder.WriteElement("onlyauth", Force ? 0 : 1);

            xmlRequestBuilder.WriteEndElement(); // </message>
        }
    }
}