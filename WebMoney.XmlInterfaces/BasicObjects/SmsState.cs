namespace WebMoney.XmlInterfaces.BasicObjects
{
    public enum SmsState
    {
        /// <summary>
        /// waiting to be sent
        /// </summary>
        BUFFERED,

        /// <summary>
        /// is being sent to the operator
        /// </summary>
        SENDING,

        /// <summary>
        /// sent to the operator
        /// </summary>
        SENDED,

        /// <summary>
        /// delivered
        /// </summary>
        DELIVERED,

        /// <summary>
        /// not delivered
        /// </summary>
        NON_DELIVERED,

        /// <summary>
        /// postponed to repeat
        /// </summary>
        SUSPENDED,

        /// <summary>
        /// The message is postponed until the verification of the SIM card identification according to the HLR.
        /// </summary>
        HLRPENDING,

        /// <summary>
        /// verification of the SIM card identification has identified the mismatches according to the HLR (to confirm the change of the SIM card the user can use the link https://security.wmtransfer.com/asp/resetphs.asp)
        /// </summary>
        HLRMISMATCH
    }
}