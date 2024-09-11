using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ChipTagValidator
{
    public class VisaParser
    {
        private XmlDocument xmlDocument = new XmlDocument();

        public List<string> LoadXmlFile(string filepath) { 
            xmlDocument.Load(filepath);
            List<string> tags = new List<string>();
            var tagNodes = xmlDocument.GetElementsByTagName("tag");
            var lenNodes = xmlDocument.GetElementsByTagName("taglength");
            var valueNodes = xmlDocument.GetElementsByTagName("tagvalue");
            var interfaceNotes = xmlDocument.GetElementsByTagName("tagname");

            StringBuilder sb = new StringBuilder();
            if (tagNodes.Count == lenNodes.Count && tagNodes.Count == valueNodes.Count) {
                for (int i = 0; i < tagNodes.Count; i++) {
                    string x = tagNodes[i].InnerText + lenNodes[i].InnerText + valueNodes[i].InnerText + interfaceNotes[i].Attributes.GetNamedItem("category").InnerText;
                    tags.Add(x);
                }
            }
            else
            {
                Console.WriteLine("error lenghts not equal");
            }
            return tags;
        }
    }
}
