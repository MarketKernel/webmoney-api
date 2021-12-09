using System;
using System.Globalization;
using System.Xml;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Core;
using WebMoney.XmlInterfaces.Core.Exceptions;

namespace WebMoney.XmlInterfaces.Utilities
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class WmXmlPackage : XmlPackage
    {
        public WmXmlPackage(XmlPackage xmlPackage)
            : base(xmlPackage)
        {
        }

        public WmXmlPackage(XmlNode xmlNode)
            : base(xmlNode)
        {
        }

        internal Amount SelectAmount(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectAmountIfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        internal Amount? SelectAmountIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!Amount.TryParse(text, out var amount))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format.", text, xPath));

            return amount;
        }

        internal WmId SelectWmId(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectWmIdIfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public WmId? TrySelectWmId(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            WmId? value;

            try
            {
                value = SelectWmIdIfExists(xPath);
            }
            catch (FormatException)
            {
                return null;
            }

            return value;
        }

        public WmId? SelectWmIdIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!WmId.TryParse(text, out var wmId))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format.", text, xPath));

            return wmId;
        }

        internal Purse SelectPurse(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectPurseIfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public Purse? SelectPurseIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!Purse.TryParse(text, out var purse))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format.", text, xPath));

            return purse;
        }

        internal WmDateTime SelectWmDateTime(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectWmDateTimeIfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public WmDateTime? SelectWmDateTimeIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (text.Equals("0000-00-00 00:00:00", StringComparison.OrdinalIgnoreCase))
                return default(DateTime);

            if (!WmDateTime.TryParseServerString(text, out var wmDateTime))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format.", text, xPath));

            return wmDateTime;
        }

        internal WmDate SelectWmDate(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectWmDateIfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public WmDate? SelectWmDateIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!WmDate.TryParseServerString(text, out var wmDate))
                throw new FormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format.", text, xPath));

            return wmDate;
        }

        public object SelectEnumFromIntegerIfExists(Type type, string xPath)
        {
            if (null == type)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if ("null".Equals(text, StringComparison.Ordinal))
                return null;

            var int32 = SelectInt32(xPath);

            var enumValue = Enum.ToObject(type, int32);

            if (!Enum.IsDefined(type, enumValue))
                throw new IncorrectFormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format. Expected a '{2}' expression.",
                    text, xPath, type.Name));

            return enumValue;
        }
    }
}