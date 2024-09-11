using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChipTagValidator
{
    public class MastercardXmlParser : XmlParser
    {
        public override List<string> Parse(string filePath)
        {
            List<string> result = new List<string>();
            XmlDocument doc = LoadXmlFile(filePath);
            XmlNodeList rootNodes = doc.GetElementsByTagName("WORKSHEET");
            foreach (XmlNode node in rootNodes) {
                string type = node.Attributes.GetNamedItem("NAME").InnerText;
                foreach (XmlNode node2 in node.ChildNodes) {
                    string tagName = node2.Attributes.GetNamedItem("TAG").InnerText;
                    if (tagName.Length > 0)
                    {
                        string value = node2.Attributes.GetNamedItem("VALUE").InnerText;
                        int lengthInt = value.Length / 2;
                        string length = lengthInt < 10 ? "0"+ lengthInt : lengthInt.ToString();
                        result.Add(tagName + length + value + "{" + type + "}");
                    }
                }
            }
            return result;
        }
    }
}
