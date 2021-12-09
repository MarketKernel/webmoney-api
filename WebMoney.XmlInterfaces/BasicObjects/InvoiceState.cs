using System;

namespace WebMoney.XmlInterfaces.BasicObjects
{
    [Serializable]
    public enum InvoiceState
    {
        /// <summary>
        /// Unpaid.
        /// </summary>
        NotPaid = 0,

        /// <summary>
        /// Paid with protection.
        /// </summary>
        PaidProtection,

        /// <summary>
        /// Fully paid or paid without protection.
        /// </summary>
        Paid,

        /// <summary>
        /// Rejected.
        /// </summary>
        Refusal
    }
}