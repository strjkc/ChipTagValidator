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
            foreach (XmlNode typeNode in rootNodes) {
                string type = typeNode.Attributes.GetNamedItem(tagDescription).InnerText;
                foreach (XmlNode tagNode in typeNode.ChildNodes) {
                    string tagName = tagNode.Attributes.GetNamedItem(MastercardXmlParser.tagName).InnerText;
                    if (tagName != "")
                    {
                        TagBuilder tagBuilder = new TagBuilder();
                        tagBuilder.StandardTagname = tagName;
                        tagBuilder.Value = tagNode.Attributes.GetNamedItem(tagValue).InnerText;
                        tagBuilder.Length = (tagBuilder.Value.Length / 2).ToString("X2");
                        tagBuilder.IsCless = type.Contains("contactless");
                        result.Add(tagBuilder.BuildTag());
                    }
                }
            }
            return result;
        }
    }
}
