using System;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X16. Creating a purse.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class RecentPurse : WmResponse
    {
        /// <summary>
        /// Unique purse number in the WebMoney system.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Purse number.
        /// </summary>
        public Purse Purse { get; protected set; }

        /// <summary>
        /// Amount on the purse.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// Purse name.
        /// </summary>
        public Description Description { get; protected set; }

        protected override void Inspect(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            int errorNumber = xmlPackage.SelectInt32("retval");

            if (0 != errorNumber)
                throw new OriginalPurseException(errorNumber, xmlPackage.SelectString("retdesc"));
        }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            Id = wmXmlPackage.SelectInt32("purse/@id");
            Purse = wmXmlPackage.SelectPurse("purse/pursename");
            Amount = wmXmlPackage.SelectAmount("purse/amount");
            Description = (Description) wmXmlPackage.SelectNotEmptyString("purse/desc");
        }
    }
}