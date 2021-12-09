using System;

namespace WebMoney.XmlInterfaces.BasicObjects
{
    [
    Serializable
    ]
    public enum AuthorizationMode
    {
        None,
        Merchant,
        Classic,
        Light,
    }
}