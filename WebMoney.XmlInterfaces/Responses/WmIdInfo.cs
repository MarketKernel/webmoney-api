using System;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// 
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class WmIdInfo
    {
        /// <summary>
        /// WMID.
        /// </summary>
        public WmId WmId { get; protected set; }

        /// <summary>
        /// Unique name (nick) for this WMID.
        /// </summary>
        public string Alias { get; protected set; }

        /// <summary>
        /// Additional information about WMID.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Date and time when WMID was registered in the system.
        /// </summary>
        public WmDateTime RegistrationDate { get; protected set; }

        internal void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            WmId = wmXmlPackage.SelectWmId("@wmid");
            Alias = wmXmlPackage.SelectString("@nickname");
            Description = wmXmlPackage.SelectString("@info");
            RegistrationDate = wmXmlPackage.SelectWmDateTime("@datereg");
        }
    }
}