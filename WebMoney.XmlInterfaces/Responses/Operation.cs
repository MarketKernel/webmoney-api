using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Utilities;

namespace WebMoney.XmlInterfaces.Responses
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public abstract class Operation
    {
        /// <summary>
        ///  Unique Operation number in the WebMoney system.
        /// </summary>
        public long PrimaryId { get; protected set; }

        /// <summary>
        /// Internal number for the Operation in the WebMoney system.
        /// </summary>
        public long SecondaryId { get; protected set; }

        /// <summary>
        /// Recipient purse.
        /// </summary>
        public Purse TargetPurse { get; protected set; }

        /// <summary>
        /// Operation amount.
        /// </summary>
        public Amount Amount { get; protected set; }

        /// <summary>
        /// Description of the product or service.
        /// </summary>
        public Description Description { get; protected set; }

        /// <summary>
        /// Date and time of the Operation creation.
        /// </summary>
        public WmDateTime CreateTime { get; protected set; }

        /// <summary>
        /// Date and time of the latest Operation status change.
        /// </summary>
        public WmDateTime UpdateTime { get; protected set; }

        internal abstract void Fill(WmXmlPackage wmXmlPackage);
    }
}