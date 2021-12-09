using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X6. Sending message to random WM-identifier via internal mail.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class RecentMessage : WmResponse
    {
        /// <summary>
        /// Unique message number in WebMoney registration system.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// WM identifier of message receiver.
        /// </summary>
        public WmId WmId { get; protected set; }

        /// <summary>
        /// Message subject.
        /// </summary>
        public Description Subject { get; protected set; }

        /// <summary>
        /// Message text.
        /// </summary>
        public Message Content { get; protected set; }

        /// <summary>
        /// Date and time of message delivery.
        /// </summary>
        public WmDateTime CreateTime { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new OriginalMessageException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = wmXmlPackage.SelectInt32("message/@id");
            WmId = wmXmlPackage.SelectWmId("message/receiverwmid");
            Subject = (Description) wmXmlPackage.SelectString("message/msgsubj");
            Content = (Message) wmXmlPackage.SelectNotEmptyString("message/msgtext");
            CreateTime = wmXmlPackage.SelectWmDateTime("message/datecrt");
        }
    }
}