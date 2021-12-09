using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using WebMoney.XmlInterfaces.Core.Exceptions;

namespace WebMoney.XmlInterfaces.Core
{
#if DEBUG
#else
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class XmlPackage
    {
        private readonly XmlNamespaceManager _namespaceManager;
        private readonly XmlNode _xmlNode;

        public string OuterXml => _xmlNode?.OwnerDocument?.OuterXml;

        public string NodeOuterXml => _xmlNode.OuterXml;

        public XmlPackage(XmlPackage prototype)
        {
            if (null == prototype)
                throw new ArgumentNullException(nameof(prototype));

            _xmlNode = prototype._xmlNode;
            _namespaceManager = prototype._namespaceManager;
        }

        public XmlPackage(XmlDocument xmlDocument)
        {
            if (null == xmlDocument)
                throw new ArgumentNullException(nameof(xmlDocument));

            XmlElement documentElement = xmlDocument.DocumentElement;

            _xmlNode = documentElement ?? throw new InvalidOperationException("Root element is missing.");

            var nameTable = xmlDocument.NameTable;

            if (null == nameTable)
                throw new InvalidOperationException("null == nameTable");

            _namespaceManager = new XmlNamespaceManager(nameTable);
        }

        public XmlPackage(XmlNode xmlNode, XmlNamespaceManager namespaceManager = null)
        {
            _xmlNode = xmlNode ?? throw new ArgumentNullException(nameof(xmlNode));

            if (null != namespaceManager)
                _namespaceManager = namespaceManager;
            else
            {
                XmlDocument xmlDocument = xmlNode.OwnerDocument;

                if (null == xmlDocument)
                    throw new InvalidOperationException("null == xmlNode.OwnerDocument");

                XmlNameTable nameTable = xmlDocument.NameTable;

                if (null == nameTable)
                    throw new InvalidOperationException("null == nameTable");

                _namespaceManager = new XmlNamespaceManager(nameTable);
            }
        }

        public static XmlPackage CreateXmlPackage(XmlReader xmlReader)
        {
            if (null == xmlReader)
                throw new ArgumentNullException(nameof(xmlReader));

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);

            return new XmlPackage(xmlDocument);
        }

        public static XmlPackage CreateXmlPackage(string xmlString)
        {
            if (null == xmlString)
                throw new ArgumentNullException(nameof(xmlString));

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            return new XmlPackage(xmlDocument);
        }

        public void AddNamespace(string prefix, string uri)
        {
            if (null == prefix)
                throw new ArgumentNullException(nameof(prefix));

            if (null == uri)
                throw new ArgumentNullException(nameof(uri));

            _namespaceManager.AddNamespace(prefix, uri);
        }

        // READING

        public bool Exists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return null != _xmlNode.SelectSingleNode(xPath, _namespaceManager);
        }

        public XmlPackage Select(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var xmlPackage = SelectIfExists(xPath);

            if (null == xmlPackage)
                throw new MissingParameterException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The element '{0}' does not exists.",
                        xPath));

            return xmlPackage;
        }

        public XmlPackage SelectIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var xmlNode = _xmlNode.SelectSingleNode(xPath, _namespaceManager);

            if (null == xmlNode)
                return null;

            return new XmlPackage(xmlNode, _namespaceManager);
        }

        public IList<XmlPackage> SelectList(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var xmlPackageList = new List<XmlPackage>();

            var xmlNodeList = _xmlNode.SelectNodes(xPath, _namespaceManager);

            if (xmlNodeList != null)
                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    xmlPackageList.Add(new XmlPackage(xmlElement, _namespaceManager));
                }

            return xmlPackageList;
        }

        public string SelectString(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var xmlNode = _xmlNode.SelectSingleNode(xPath, _namespaceManager);

            if (null == xmlNode)
                throw new MissingParameterException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The element '{0}' does not exists.",
                        xPath));

            return xmlNode.InnerText;
        }

        public string SelectStringIfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var xmlNode = _xmlNode.SelectSingleNode(xPath, _namespaceManager);
            return xmlNode?.InnerText;
        }

        public string SelectNotEmptyString(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = SelectString(xPath);

            if (string.IsNullOrEmpty(text))
                throw new IncorrectFormatException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' is empty. Expected a non-empty string.", xPath));

            return text;
        }

        public string TrySelectNotEmptyString(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = SelectStringIfExists(xPath);

            if (string.IsNullOrEmpty(text))
                return null;

            return text;
        }

        public bool SelectBool(string xPath, bool isDigit = true)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectBoolIfExists(xPath, isDigit);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public bool? SelectBoolIfExists(string xPath, bool isDigit = true)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            if (isDigit)
            {
                byte? @byte = SelectUInt8IfExists(xPath);

                if (!@byte.HasValue)
                    return null;

                if (@byte.Value > 1)
                    throw new OutOfRangeException(string.Format(CultureInfo.InvariantCulture,
                        "The value '{0}' of the element '{1}' is out of range: an integer from 0 to 1 is expected.",
                        @byte, xPath));

                return @byte == 1;
            }

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!bool.TryParse(text.ToUpper(CultureInfo.InvariantCulture), out var @bool))
                throw new IncorrectFormatException(
                    string.Format(CultureInfo.InvariantCulture,
                        "The value '{0}' of the element '{1}' is not in a correct format. Expected a 'Bool' expression.",
                        text, xPath));

            return @bool;
        }

        public sbyte SelectInt8(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectInt8IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public sbyte? SelectInt8IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (sbyte?)SelectIntegerIfExists(xPath, sbyte.MinValue, sbyte.MaxValue);
        }

        public byte SelectUInt8(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectUInt8IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public byte? SelectUInt8IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (byte?)SelectUnsignedIntegerIfExists(xPath, byte.MaxValue);
        }

        public short SelectInt16(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectInt16IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public short? SelectInt16IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (short?)SelectIntegerIfExists(xPath, short.MinValue, short.MaxValue);
        }

        public ushort SelectUInt16(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectUInt16IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public ushort? SelectUInt16IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (ushort?)SelectUnsignedIntegerIfExists(xPath, ushort.MaxValue);
        }

        public int SelectInt32(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectInt32IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public int? SelectInt32IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (int?) SelectIntegerIfExists(xPath, int.MinValue, int.MaxValue);
        }

        public uint SelectUInt32(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectUInt32IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(string.Format(CultureInfo.InvariantCulture,
                    "The element '{0}' does not exists or has a null value.", xPath));

            return value.Value;
        }

        public uint? SelectUInt32IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            return (uint?) SelectUnsignedIntegerIfExists(xPath, uint.MaxValue);
        }

        public long SelectInt64(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectInt64IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The element '{0}' does not exists or has a null value.",
                        xPath));

            return value.Value;
        }

        public long? SelectInt64IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out var @long))
                throw new IncorrectFormatException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The value '{0}' of the element '{1}' is not in a correct format: a long integer is expected.",
                        text,
                        xPath));

            return @long;
        }

        public ulong SelectUInt64(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectUInt64IfExists(xPath);

            if (!value.HasValue)
                throw new MissingParameterException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The element '{0}' does not exists or has a null value.",
                        xPath));

            return value.Value;
        }

        public ulong? SelectUInt64IfExists(string xPath)
        {
            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            if (!ulong.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out var @ulong))
                throw new IncorrectFormatException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The value '{0}' of the element '{1}' is not in a correct format: a positive long integer is expected.",
                        text,
                        xPath));

            return @ulong;
        }

        public object SelectEnum(Type type, string xPath)
        {
            if (null == type)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            var value = SelectEnumIfExists(type, xPath);

            if (null == value)
                throw new MissingParameterException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The element '{0}' does not exists or has a null value.",
                        xPath));

            return value;
        }

        public object SelectEnumIfExists(Type type, string xPath)
        {
            if (null == type)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrEmpty(xPath))
                throw new ArgumentNullException(nameof(xPath));

            string text = TrySelectNotEmptyString(xPath);

            if (null == text)
                return null;

            try
            {
                return Enum.Parse(type, text, true);
            }
            catch (ArgumentException)
            {
                throw new IncorrectFormatException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is not in a correct format. Expected a '{2}' expression.",
                    text, xPath, type.Name));
            }
        }

        // WRITING

        public void Import(XmlPackage xmlPackage)
        {
            if (null == xmlPackage)
                throw new ArgumentNullException(nameof(xmlPackage));

            if (_xmlNode.OwnerDocument != null)
            {
                XmlNode newChild = _xmlNode.OwnerDocument.ImportNode(xmlPackage._xmlNode, true);
                _xmlNode.InsertAfter(newChild, _xmlNode.LastChild);
            }
            else
                throw new InvalidOperationException("_xmlNode.OwnerDocument != null");
        }

        public void SetInnerText(string xPath, string value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            Select(xPath)._xmlNode.InnerText = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetInnerText(string xPath, bool value, bool digit = true)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            if (digit)
                SetInnerText(xPath, value ? "1" : "0");
            else
                SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, sbyte value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, byte value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, short value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, ushort value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, int value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, uint value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, long value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetInnerText(string xPath, ulong value)
        {
            if (null == xPath)
                throw new ArgumentNullException(nameof(xPath));

            SetInnerText(xPath, value.ToString(CultureInfo.InvariantCulture));
        }
        
        public void WriteContentTo(XmlWriter xmlWriter)
        {
            if (null == xmlWriter)
                throw new ArgumentNullException(nameof(xmlWriter));

            _xmlNode?.OwnerDocument?.WriteContentTo(xmlWriter);
        }

        private long? SelectIntegerIfExists(string xPath, long minValue, long maxValue)
        {
            var @long = SelectInt64IfExists(xPath);

            if (!@long.HasValue)
                return null;

            if (@long.Value < minValue || @long.Value > maxValue)
                throw new OutOfRangeException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is out of range: an integer from {2} to {3} is expected.",
                    @long, xPath, minValue, maxValue));

            return @long;
        }

        private ulong? SelectUnsignedIntegerIfExists(string xPath, ulong maxValue)
        {
            var @ulong = SelectUInt64IfExists(xPath);

            if (!@ulong.HasValue)
                return null;

            if (@ulong.Value > maxValue)
                throw new OutOfRangeException(string.Format(CultureInfo.InvariantCulture,
                    "The value '{0}' of the element '{1}' is out of range: an integer from 0 to {2} is expected.",
                    @ulong, xPath, maxValue));

            return @ulong;
        }
    }
}