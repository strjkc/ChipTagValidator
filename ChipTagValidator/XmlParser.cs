using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public abstract class XmlParser
    {
        //da li parsira vpa ili cpv
        //primi fajl nadje maticni xml tag, unutar njega
        //

        protected string _rootTag;

        protected XmlDocument LoadXmlFile(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
            }
            catch (Exception ex) {
                
                Console.WriteLine(ex.Message);
            }
            return xmlDoc;
        }

        protected XmlNodeList GetRootElements(XmlDocument doc) { 
            
            return doc.GetElementsByTagName(_rootTag);
        }



        public abstract List<TagModel> Parse(string filePath);
    }
}
