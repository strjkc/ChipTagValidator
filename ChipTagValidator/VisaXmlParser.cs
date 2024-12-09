using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ChipTagValidator.Interfaces;
using ChipTagValidator.Models;
using Serilog;

namespace ChipTagValidator
{
    public class VisaXmlParser: XmlParser
    {
        //TODO include template tags 


        /* Visa Model:
            <tagelement>
                <tagname category="qVSDC">Application Capabilities</tagname>
                <tag>DF01</tag>
                <templatetag>BF5B</templatetag>
                <taglength>02</taglength>
                <tagvalue>8000</tagvalue>
            </tagelement>
            <tagelement>
                <tagname category="qVSDC">Form Factor Indicator</tagname>
                <tag>9F6E</tag>
                <taglength>04</taglength>
                <tagvalue>40700D00</tagvalue>
            </tagelement>
         */
        //TODO extract constants to json config

        private string _tagelement = "tag";
        private string _lengthElement = "taglength";
        private string _valueElement = "tagvalue";
        private string _typeElement = "tagname";
        private string _attribute = "category";

        public override List<TagModel> Parse(string filePath) {
            XmlDocument doc = LoadFile<XmlDocument>(filePath);
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
                    tagBuilder.IsCless = typeNodes[i].Attributes.GetNamedItem(_attribute).InnerText.Equals("qVSDC");
                    result.Add(tagBuilder.BuildTag());
                }
            }
            else
            {
                Log.Error("Mimatch in number of tags");
                Log.Error($"tagNodes: {tagNodes.Count},  lengthNodes: {lengthNodes.Count}, valueNodes:  {valueNodes.Count}");
                throw new Exception("Mimatch in number of tags");
                //throw exception that there is a mismatch in the amount of tags the form is not valid
            }
            StringBuilder sb = new StringBuilder();
            foreach (TagModel tag in result)
            {
                sb.Append($"StandardTagname: {tag.StandardTagname} InternalTagName: {tag.InternalTagName} TemplateTag: {tag.TemplateTag} TagLength: {tag.Length} TagValue: {tag.Value} IsCless: {tag.IsCless}, ");
            }
            Log.Information($"Parsed from XML file: {sb.ToString()}");
            return result;
        }
    }
}
