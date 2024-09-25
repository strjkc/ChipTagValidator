using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public class MastercardXmlParser : XmlParser
    {
        private const string rootTag = "WORKSHEET";
        private const string tagDescription = "NAME";
        private const string tagName = "TAG";
        private const string tagValue = "VALUE";

        public override List<TagModel> Parse(string filePath)
        {
            List<TagModel> result = new List<TagModel>();
            XmlDocument doc = LoadXmlFile(filePath);
            XmlNodeList rootNodes = doc.GetElementsByTagName(rootTag);
            foreach (XmlNode node in rootNodes) {
                string type = node.Attributes.GetNamedItem(tagDescription).InnerText;
                foreach (XmlNode node2 in node.ChildNodes) {
                    string tagName = node2.Attributes.GetNamedItem(MastercardXmlParser.tagName).InnerText;
                    if (tagName.Length > 0)
                    {
                        TagBuilder tagBuilder = new TagBuilder();
                        string value = node2.Attributes.GetNamedItem(tagValue).InnerText;
                        tagBuilder.Value = value;
                        int lengthInt = value.Length / 2;


                        string length = lengthInt < 10 ? "0"+ lengthInt : lengthInt.ToString();
//                        result.Add(tagName + length + value + "{" + type + "}");
                    }
                }
            }
            return result;
        }
    }
}
