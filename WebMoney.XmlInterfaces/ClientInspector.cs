using System;
using System.Globalization;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.XmlInterfaces
{
    /// <summary>
    /// Interface X19. Verifying personal information for the owner of a WM identifier.
    /// </summary>
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public class ClientInspector : WmRequest<ClientEvidence>
    {
        private string _cryptoCurrencyAddress;

        
        protected override string ClassicUrl => "https://apipassport.webmoney.ru/XMLCheckUser.aspx";

        protected override string LightUrl => "https://apipassportcrt.webmoney.ru/XMLCheckUserCert.aspx";

        /// <summary>
        /// Type of operation.
        /// </summary>
        public ExchangeType OperationType { get; set; }

        /// <summary>
        /// Direction of the operation.
        /// </summary>
        public bool Output { get; set; } = true;

        /// <summary>
        /// The type of WM purse from or to which the transfer is taking place.
        /// </summary>
        public WmCurrency Currency { get; set; }

        /// <summary>
        /// Transfer amount.
        /// </summary>
        public Amount Amount { get; set; }

        /// <summary>
        /// User's WMID.
        /// </summary>
        public WmId Wmid { get; set; }

        /// <summary>
        /// Passport number.
        /// </summary>
        public Description PassportNumber { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public Description SecondName { get; set; }

        /// <summary>
        /// User's first name.
        /// </summary>
        public Description FirstName { get; set; }

        /// <summary>
        /// Bank name.
        /// </summary>
        public Description BankName { get; set; }


        /// <summary>
        /// Bank account number.
        /// </summary>
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Bank card number.
        /// </summary>
        public BankCard CardNumber { get; set; }

        /// <summary>
        /// Payment system name.
        /// </summary>
        public PaymentSystem PaymentSystem { get; set; }

        /// <summary>
        /// User ID for the payment system.
        /// </summary>
        public Description PaymentSystemId { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Crypto name.
        /// </summary>
        public CryptoCurrencyType CryptoCurrencyType { get; set; }

        /// <summary>
        /// Address to get cryptocurrencies.
        /// </summary>
        public string CryptoCurrencyAddress
        {
            get => _cryptoCurrencyAddress;
            set => _cryptoCurrencyAddress = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected internal ClientInspector()
        {
        }

        /// <summary>
        /// Cash
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="passportNumber">Passport number.</param>
        /// <param name="secondName">User's last name.</param>
        /// <param name="firstName">User's first name.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, Description passportNumber, Description secondName, Description firstName)
        {
            OperationType = ExchangeType.Cash;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            PassportNumber = passportNumber;
            FirstName = firstName;
            SecondName = secondName;
        }

        /// <summary>
        /// OfflineSystem
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="secondName">User's last name.</param>
        /// <param name="firstName">User's first name.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, Description secondName, Description firstName)
        {
            OperationType = ExchangeType.OfflineSystem;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            SecondName = secondName;
            FirstName = firstName;
        }

        /// <summary>
        /// BankAccount
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="secondName">User's last name.</param>
        /// <param name="firstName">User's first name.</param>
        /// <param name="bankName">Bank name.</param>
        /// <param name="bankAccount">Bank account number.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, Description secondName, Description firstName, Description bankName, BankAccount bankAccount)
        {
            OperationType = ExchangeType.BankAccount;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            SecondName = secondName;
            FirstName = firstName;
            BankName = bankName;
            BankAccount = bankAccount;
        }

        /// <summary>
        /// BankCard
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="secondName">User's last name.</param>
        /// <param name="firstName">User's first name.</param>
        /// <param name="bankName">Bank name.</param>
        /// <param name="cardNumber">Bank card number.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, Description secondName, Description firstName, Description bankName, BankCard cardNumber)
        {
            OperationType = ExchangeType.BankCard;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            SecondName = secondName;
            FirstName = firstName;
            BankName = bankName;
            CardNumber = cardNumber;
        }

        /// <summary>
        /// InternetSystem
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="paymentSystem">Payment system name.</param>
        /// <param name="paymentSystemId">User ID for the payment system.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, PaymentSystem paymentSystem, Description paymentSystemId)
        {
            OperationType = ExchangeType.InternetSystem;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            PaymentSystem = paymentSystem;
            PaymentSystemId = paymentSystemId;
        }

        /// <summary>
        /// SMS
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="phone">Phone number.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, string phone)
        {
            OperationType = ExchangeType.Sms;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            Phone = phone;
        }

        /// <summary>
        /// Moble
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="phone">Phone number.</param>
        /// <param name="mobile">Is Mobile phone recharge or SMS-payment.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, string phone, bool mobile)
        {
            OperationType = mobile ? ExchangeType.Mobile : ExchangeType.Sms;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            Phone = phone;
        }

        /// <summary>
        /// Crypto
        /// </summary>
        /// <param name="currency">The type of WM purse from or to which the transfer is taking place.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="wmid">User's WMID.</param>
        /// <param name="cryptoCurrencyType"></param>
        /// <param name="address">Address to get cryptocurrencies.</param>
        public ClientInspector(WmCurrency currency, Amount amount, WmId wmid, CryptoCurrencyType cryptoCurrencyType, string address)
        {
            OperationType = ExchangeType.CryptoCurrency;
            Currency = currency;
            Amount = amount;
            Wmid = wmid;
            CryptoCurrencyType = cryptoCurrencyType;
            CryptoCurrencyAddress = address ?? throw new ArgumentNullException(nameof(address));
        }

        protected override string BuildMessage(ulong requestNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", requestNumber, (int)OperationType, Wmid);
        }

        protected override void BuildXmlHead(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartDocument();

            xmlRequestBuilder.WriteStartElement("passport.request"); // <passport.request>

            ulong requestNumber = Initializer.GetRequestNumber();

            xmlRequestBuilder.WriteElement("reqn", requestNumber);

            if (AuthorizationMode.Classic == Initializer.Mode)
            {
                xmlRequestBuilder.WriteElement("signerwmid", Initializer.Id.ToString());
                xmlRequestBuilder.WriteElement("sign", Initializer.Sign(BuildMessage(requestNumber)));
            }
        }

        protected override void BuildXmlBody(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteStartElement("operation"); // <operation>

            xmlRequestBuilder.WriteElement("type", (int)OperationType);
            xmlRequestBuilder.WriteElement("direction", Output ? 1 : 2);
            xmlRequestBuilder.WriteElement("pursetype", "WM" + Purse.CurrencyToLetter(Currency));
            xmlRequestBuilder.WriteElement("amount", Amount.ToString());

            xmlRequestBuilder.WriteEndElement(); // </operation>

            xmlRequestBuilder.WriteStartElement("userinfo"); // <userinfo>

            xmlRequestBuilder.WriteElement("wmid", Wmid.ToString());

            switch (OperationType)
            {
                case ExchangeType.Cash:
                    xmlRequestBuilder.WriteElement("pnomer", PassportNumber);
                    xmlRequestBuilder.WriteElement("fname", SecondName);
                    xmlRequestBuilder.WriteElement("iname", FirstName);
                    break;
                case ExchangeType.OfflineSystem:
                    xmlRequestBuilder.WriteElement("fname", SecondName);
                    xmlRequestBuilder.WriteElement("iname", FirstName);
                    break;
                case ExchangeType.BankAccount:
                    xmlRequestBuilder.WriteElement("fname", SecondName);
                    xmlRequestBuilder.WriteElement("iname", FirstName);
                    xmlRequestBuilder.WriteElement("bank_name", BankName);
                    xmlRequestBuilder.WriteElement("bank_account", BankAccount.ToString());
                    break;
                case ExchangeType.BankCard:
                    xmlRequestBuilder.WriteElement("fname", SecondName);
                    xmlRequestBuilder.WriteElement("iname", FirstName);
                    xmlRequestBuilder.WriteElement("bank_name", BankName);
                    xmlRequestBuilder.WriteElement("card_number", CardNumber.ToString());
                    break;
                case ExchangeType.InternetSystem:
                {
                    string paymentSystem;

                    switch (PaymentSystem)
                    {
                        case PaymentSystem.PayPal:
                            paymentSystem = "paypal.com";
                            break;
                        case PaymentSystem.Skrill:
                            paymentSystem = "skrill.com";
                            break;
                        case PaymentSystem.Alipay:
                            paymentSystem = "alipay.com";
                            break;
                        case PaymentSystem.Qiwi:
                            paymentSystem = "qiwi.ru";
                            break;
                        case PaymentSystem.YandexMoney:
                            paymentSystem = "pamoney.yandex.ru";
                            break;
                        default:
                            throw new InvalidOperationException("PaymentSystem == " + PaymentSystem);
                    }

                    xmlRequestBuilder.WriteElement("emoney_name", paymentSystem);
                    xmlRequestBuilder.WriteElement("emoney_id", PaymentSystemId);
                }
                    break;
                case ExchangeType.Sms:
                    xmlRequestBuilder.WriteElement("phone", Phone);
                    break;
                case ExchangeType.Mobile:
                    xmlRequestBuilder.WriteElement("phone", Phone);
                    break;
                case ExchangeType.CryptoCurrency:
                {
                    string cryptoCurrencyName;

                    switch (CryptoCurrencyType)
                    {
                        case CryptoCurrencyType.Bitcoin:
                            cryptoCurrencyName = "bitcoin";
                            break;
                        case CryptoCurrencyType.BitcoinCash:
                            cryptoCurrencyName = "bitcoincash";
                            break;
                        default:
                            throw new InvalidOperationException("CryptoCurrencyType == " + CryptoCurrencyType);
                    }

                    xmlRequestBuilder.WriteElement("crypto_name", cryptoCurrencyName);
                    xmlRequestBuilder.WriteElement("crypto_address", CryptoCurrencyAddress);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            xmlRequestBuilder.WriteEndElement(); // </userinfo>
        }

        protected override void BuildXmlFoot(XmlRequestBuilder xmlRequestBuilder)
        {
            if (null == xmlRequestBuilder)
                throw new ArgumentNullException(nameof(xmlRequestBuilder));

            xmlRequestBuilder.WriteEndElement(); // </passport.request>

            xmlRequestBuilder.WriteEndDocument();
        }
    }
}