using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X5. Completing a code-protected transaction. Entering a protection code.
    /// Interface X13. Recalling incomplete protected transaction.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class ProtectionReport : WmResponse
    {
        /// <summary>
        /// Defines unique transaction number in the WebMoney system
        /// </summary>
        public long PrimaryId { get; protected set; }

        /// <summary>
        ///  Defines a auxiliary transaction number in the WebMoney system
        /// </summary>
        public long SecondaryId { get; protected set; }

        /// <summary>
        /// Transfer type.
        /// </summary>
        public TransferType TransferType { get; protected set; }

        /// <summary>
        /// Date and time of the latest transaction status change.
        /// </summary>
        public WmDateTime UpdateTime { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            PrimaryId = wmXmlPackage.SelectInt64("operation/@id");
            SecondaryId = wmXmlPackage.SelectInt64("operation/@ts");
            TransferType = (TransferType) wmXmlPackage.SelectInt32("operation/opertype");
            UpdateTime = wmXmlPackage.SelectWmDateTime("operation/dateupd");
        }
    }
}