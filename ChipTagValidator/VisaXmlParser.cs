using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public override List<String> Parse(string filePath) {
            XmlDocument doc = LoadXmlFile(filePath);
            List<string> result = new List<string>();
            XmlNodeList tagNodes = doc.GetElementsByTagName(_tagelement);
            XmlNodeList lengthNodes = doc.GetElementsByTagName(_lengthElement);
            XmlNodeList valueNodes = doc.GetElementsByTagName(_valueElement);
            XmlNodeList typeNodes = doc.GetElementsByTagName(_typeElement);
            if (tagNodes.Count == lengthNodes.Count && tagNodes.Count == valueNodes.Count)
            {
                for (int i = 0; i< tagNodes.Count; i++)
                {
                    string tag = tagNodes[i].InnerText;
                    string length = lengthNodes[i].InnerText;
                    string value = valueNodes[i].InnerText;
                    string type = typeNodes[i].Attributes.GetNamedItem(_attribute).InnerText;
                    result.Add(tagNodes[i].InnerText + lengthNodes[i].InnerText + valueNodes[i].InnerText + "{" + type + "}");
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
