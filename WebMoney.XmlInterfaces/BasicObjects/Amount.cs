using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using log4net;

namespace WebMoney.XmlInterfaces.BasicObjects
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    [Serializable]
    public struct Amount : IXmlSerializable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Amount));

        private const string DecimalSeparator = ".";
        private const string Format = "0.##";

        private decimal _value;

        private Amount(decimal amount)
        {
            _value = amount;
        }

        public static explicit operator Amount(decimal value)
        {
            decimal temp = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            return new Amount(temp);
        }

        public static implicit operator decimal(Amount value)
        {
            return value._value;
        }

        public static explicit operator Amount(double value)
        {
            double temp = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            return new Amount((decimal)temp);
        }

        public static explicit operator double(Amount value)
        {
            return (double) value._value;
        }

        public static explicit operator Amount(float value)
        {
            float temp = (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
            return new Amount((decimal)temp);
        }

        public static explicit operator float(Amount value)
        {
            return (float)value._value;
        }

        public override string ToString()
        {
            return ToString(Format);
        }

        public string ToString(string format)
        {
            if (null == format)
                throw new ArgumentNullException(nameof(format));

            return _value.ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToBankString()
        {
            var value = (int) (_value*100);

            return value.ToString("0", CultureInfo.InvariantCulture.NumberFormat);
        }

        public static Amount Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            var firstIndex = value.IndexOf(DecimalSeparator, StringComparison.Ordinal);
            var lastIndex =  value.LastIndexOf(DecimalSeparator, StringComparison.Ordinal);

            // Иногда сервер WM отдает сумму в некорректном формате (как 123.12.44). Возможно пофиксили.
            if (firstIndex != lastIndex)
            {
                Logger.Warn(value);

                string left = value.Substring(0, firstIndex + 1);
                string right = value.Substring(firstIndex + 1, value.Length - (firstIndex + 1));

                value = left + right.Replace(DecimalSeparator, string.Empty);
            }

            Amount amount;

            if (!TryParse(value, out amount))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The string '{0}' is not a valid Amount value.", value));

            return amount;
        }

        public static Amount BankParse(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            Amount amount;

            if (!TryBankParse(value, out amount))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The string '{0}' is not a valid Amount value.", value));

            return amount;
        }

        public static bool TryParse(string value, out Amount amount)
        {
            if (string.IsNullOrEmpty(value))
            {
                amount = default(Amount);
                return false;
            }

            decimal @decimal;

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out @decimal))
            {
                amount = default(Amount);
                return false;
            }

            amount = new Amount(@decimal);
            return true;
        }

        public static bool TryBankParse(string value, out Amount amount)
        {
            if (string.IsNullOrEmpty(value))
            {
                amount = default(Amount);
                return false;
            }

            int @int;

            if (!int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out @int))
            {
                amount = default(Amount);
                return false;
            }

            amount = (Amount) (@int/100M);
            return true;
        }

        public static bool operator ==(Amount lhs, Amount rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Amount lhs, Amount rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(Amount lhs, Amount rhs)
        {
            return lhs._value > rhs._value;
        }

        public static bool operator <(Amount lhs, Amount rhs)
        {
            return lhs._value < rhs._value;
        }

        public static bool operator >=(Amount lhs, Amount rhs)
        {
            return lhs._value >= rhs._value;
        }

        public static bool operator <=(Amount lhs, Amount rhs)
        {
            return lhs._value <= rhs._value;
        }

        public bool Equals(Amount other)
        {
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Amount && Equals((Amount)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            _value = Parse(reader.ReadElementContentAsString())._value;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString());
        }

        #endregion
    }
}