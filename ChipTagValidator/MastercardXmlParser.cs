using ChipTagValidator.Interfaces;
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
        private string _rootTag;
        private string _tagDescription;
        private string _tagName;
        private string _tagValue;

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

        public MastercardXmlParser() {
            MastercardXmlParserConfig config = Configuration.Config.ConfigModel.MastercardXmlParserConfig;
            _rootTag = config.RootTag;
            _tagDescription = config.TagDescription;
            _tagName = config.TagName;
            _tagValue = config.TagValue;

        }
        public override List<TagModel> Parse(string filePath)
        {
            Log.Information($"Parsing file {filePath}");
            List<TagModel> result = new List<TagModel>();
            XmlDocument doc = LoadFile<XmlDocument>(filePath);
            XmlNodeList rootNodes = doc.GetElementsByTagName(_rootTag);
            foreach (XmlNode typeNode in rootNodes) {
                string type = typeNode.Attributes.GetNamedItem(_tagDescription).InnerText;
                foreach (XmlNode tagNode in typeNode.ChildNodes) {
                    string tagName = tagNode.Attributes.GetNamedItem(_tagName).InnerText;
                    if (tagName != "")
                    {
                        TagBuilder tagBuilder = new TagBuilder();
                        tagBuilder.StandardTagname = tagName;
                        tagBuilder.Value = tagNode.Attributes.GetNamedItem(_tagValue).InnerText;
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
