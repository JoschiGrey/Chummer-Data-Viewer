using System.Xml;

namespace Chummer_Database.Overrides;

public class CustomXmlReader : XmlTextReader
{
    public CustomXmlReader(TextReader reader) : base(reader) { }
    
    public override string ReadContentAsString()
    {
        var text = base.ReadContentAsString();

        // bool TryParse accepts case-insensitive 'true' and 'false'
        if (bool.TryParse(text, out bool result))
        {
            text = XmlConvert.ToString(result);
        }

        return text;
    }
}