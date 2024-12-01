using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public class VisaXmlParser: XmlParser
    {
        //TODO: Should not be property, make private field
        private string _tagelement = "tag";
        private string _lengthElement = "taglength";
        private string _valueElement = "tagvalue";
        private string _typeElement = "tagname";
        private string _attribute = "category";

        public override List<TagModel> Parse(string filePath) {
            XmlDocument doc = LoadXmlFile(filePath);
            List<TagModel> result = new List<TagModel>();
            XmlNodeList tagNodes = doc.GetElementsByTagName(_tagelement);
            XmlNodeList lengthNodes = doc.GetElementsByTagName(_lengthElement);
            XmlNodeList valueNodes = doc.GetElementsByTagName(_valueElement);
            XmlNodeList typeNodes = doc.GetElementsByTagName(_typeElement);
            if (tagNodes.Count == lengthNodes.Count && tagNodes.Count == valueNodes.Count)
            {
                for (int i = 0; i< tagNodes.Count; i++)
                {
                    TagBuilder tagBuilder = new TagBuilder();
                    tagBuilder.StandardTagname = tagNodes[i].InnerText;
                    tagBuilder.Length = lengthNodes[i].InnerText;
                    tagBuilder.Value = valueNodes[i].InnerText;
                    // mora da ima samo qvsdc ako je cless 
                    tagBuilder.IsCless = typeNodes[i].Attributes.GetNamedItem(_attribute).InnerText.Equals("qVSDC");
                    result.Add(tagBuilder.BuildTag());
                }
            }
            else
            {
                //throw exception that there is a mismatch in the amount of tags the form is not valid
            }

            return result;
        }
    }
}
