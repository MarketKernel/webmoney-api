using System;

namespace WebMoney.XmlInterfaces.BasicObjects
{
    [Serializable]
    public enum ExchangeType
    {
        None = 0,

        /// <summary>
        /// Purchase/withdrawal of WM in cash at an exchange point. 
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Purchase/withdrawal of WM in cash through money transfer systems. 
        /// </summary>
        OfflineSystem = 2,

        /// <summary>
        ///  Purchase/withdrawal of WM from/to a bank account. 
        /// </summary>
        BankAccount = 3,

        /// <summary>
        /// Purchase/withdrawal of WM to a bank card. 
        /// </summary>
        BankCard = 4,

        /// <summary>
        /// Exchange of WM for other types of electronic currency.
        /// </summary>
        InternetSystem = 5,

        /// <summary>
        /// Funding a WM-purse by SMS (operation/direction=2 only).
        /// </summary>
        Sms = 6,

        /// <summary>
        /// Withdrawal of WM to a phone account - recharging a phone account (operation/direction=2 only).
        /// </summary>
        Mobile = 7,

        /// <summary>
        /// Exchange of WM for cryptocurrency - (operation/direction=1 only).
        /// </summary>
        CryptoCurrency = 8
    }
}