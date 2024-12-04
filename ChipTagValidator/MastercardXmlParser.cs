using ChipTagValidator.Models;
using Serilog;
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
        private const string rootTag = "WORKSHEET";
        private const string tagDescription = "NAME";
        private const string tagName = "TAG";
        private const string tagValue = "VALUE";

        //typeNode - parent nodes that contain tag types - Contact or contactless
        //tagNode - nodes that contain tags

        /* MC Format example:
           <WORKSHEET NAME="internal">
            <ELEM NAME="Application Life Cycle Data" VALUE="" TAG="9F7E"/>
            <ELEM NAME="Reference PIN" VALUE="" TAG=""/>
           </WORKSHEET>
           <WORKSHEET NAME="recordcontact">
            <ELEM NAME="Application Effective Date" VALUE="" TAG="5F25"/>
            <ELEM NAME="Application Expiration Date" VALUE="" TAG="5F24"/>
           </WORKSHEET>
           <WORKSHEET NAME="recordcontactless">
            <ELEM NAME="Application Expiration Date" VALUE="" TAG="5F24"/>
            <ELEM NAME="Application Primary Account Number" VALUE="" TAG="5A"/>
           </WORKSHEET>
         */
        public override List<TagModel> Parse(string filePath)
        {
            Log.Information($"Parsing file {filePath}");
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
                        Log.Debug($" TLV: ${tagBuilder.StandardTagname} {tagBuilder.Length} {tagBuilder.Value}, IsCless: {tagBuilder.IsCless}");
                        result.Add(tagBuilder.BuildTag());
                    }
                }
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
