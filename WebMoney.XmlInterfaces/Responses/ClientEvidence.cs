using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X19. Verifying personal information for the owner of a WM identifier.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class ClientEvidence : WmResponse
    {
        /// <summary>
        /// Unique answer ID generated each time the interface is used.
        /// </summary>
        public Description Id { get; protected set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public Description FirstName { get; protected set; }

        /// <summary>
        /// User's patronymic.
        /// </summary>
        public Description Patronymic { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = (Description)wmXmlPackage.SelectString("retid");
            FirstName = (Description)wmXmlPackage.SelectString("userinfo/iname");
            Patronymic = (Description)wmXmlPackage.SelectString("userinfo/oname");
        }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new ClientInspectorException(xmlPackage.SelectString("retid"), errorNumber,
                                                   xmlPackage.SelectString("retdesc"));
        }
    }
}