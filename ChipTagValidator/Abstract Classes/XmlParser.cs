using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ChipTagValidator.Models;
using Serilog;

namespace ChipTagValidator.Interfaces
{
    public abstract class XmlParser : IChipFileParser
    {
        //da li parsira vpa ili cpv
        //primi fajl nadje maticni xml tag, unutar njega
        //

        protected string _rootTag;

        public T LoadFile<T>(string path)
        {

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(path);
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
            }
            return (T)(object)xmlDoc;
        }

        protected XmlNodeList GetRootElements(XmlDocument doc)
        {

            return doc.GetElementsByTagName(_rootTag);
        }



        public abstract List<TagModel> Parse(string filePath);

    }
}
