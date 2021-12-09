using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.XmlInterfaces.Responses
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public abstract class Invoice : Operation
    {
        /// <summary>
        /// Invoice's serial number in the merchant's accounting system.
        /// </summary>
        public int OrderId { get; protected set; }

        /// <summary>
        /// Delivery address.
        /// </summary>
        public Description Address { get; protected set; }

        /// <summary>
        /// Maximum protection period allowed in days; An integer in the range 0 - 255; zero means that protection is prohibited.
        /// When issuing an invoice to a WM purse, this is the maximum period (in days) for returning funds.
        /// </summary>
        public byte Period { get; protected set; }

        /// <summary>
        /// Maximum valid payment period in days;
        /// An integer in the range 0 - 255; zero means that the period is undefined.
        /// </summary>
        public byte Expiration { get; protected set; }

        /// <summary>
        /// Invoice state.
        /// </summary>
        public InvoiceState InvoiceState { get; protected set; }
    }
}