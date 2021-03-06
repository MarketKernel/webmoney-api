using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
    /// <summary>
    /// Interface X15. Viewing and changing settings of "by trust" management.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    [XmlRoot(ElementName = "w3s.response")]
    public class TrustRegister : WmResponse
    {
        /// <summary>
        /// Trust list.
        /// </summary>
        public List<Trust> TrustList { get; protected set; }

        protected override void Fill(WmXmlPackage wmXmlPackage)
        {
            if (null == wmXmlPackage)
                throw new ArgumentNullException(nameof(wmXmlPackage));

            TrustList = new List<Trust>();

            var packageList = wmXmlPackage.SelectList("trustlist/trust");

            foreach (var innerPackage in packageList)
            {
                var trust = new Trust();
                trust.Fill(new WmXmlPackage(innerPackage));

                TrustList.Add(trust);
            }
        }
    }
}