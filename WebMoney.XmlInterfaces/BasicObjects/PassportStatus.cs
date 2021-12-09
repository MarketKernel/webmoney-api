using System;

namespace WebMoney.XmlInterfaces.BasicObjects
{
    [Serializable]
    public enum PassportStatus
    {
        /// <summary>
        /// Individual person.
        /// </summary>
        PrivatePerson = 1,

        /// <summary>
        /// Legal person.
        /// </summary>
        Entity = 2,
    }
}