using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;

namespace ChipTagValidator
{
    public class WordSpecParser
    {
        //TODO: these should be extracted to parameter values
        private const int expectedColumnCount = 5;
        private const int startFromTable = 5;
        private string[] invalidValues = { "n/a", "/", "", " "};
        int columnForInternalTags = 1;
        int columnForStandardTags = 3;
        int columnForTemplateTags = 4;
        int columnForMandatoryTags = 5;

        public List<TagModel> Parse(string file) {
            List<TagModel> tagModels = new List<TagModel>();

            using WordprocessingDocument doc = WordprocessingDocument.Open(file, false) 
                ?? throw new NullReferenceException("Error on opening the Word Document");
            
            // The doc can containt a lot of tables, start from the first table relevant to chip data

            var tables = doc.MainDocumentPart.Document.Body.Elements<Table>().Skip(startFromTable).ToList()
                        ?? throw new NullReferenceException("Tables not found in document");
            foreach (Table table in tables)
            {
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    // Check if the number of columns in doc is expected by the user, if not, issues in the BuildTagFromData may occur
                    if (row.ChildElements.Count != expectedColumnCount){
                        throw new ArgumentException("Number of columns doesn't match provided parameter, parsing errors may occur");
                    }
                    TagModel tag = BuildTagFromData(row);
                    tagModels.Add(tag);

                }
                }
            return tagModels;

        }


        private TagModel BuildTagFromData(TableRow row) {
            var tagBuilder = new TagBuilder();

            string internalTagText = row.ChildElements[columnForInternalTags].InnerText.Trim();
            string standardTagText = row.ChildElements[columnForStandardTags].InnerText.Trim();
            string templateTagText = row.ChildElements[columnForTemplateTags].InnerText.Trim();

            tagBuilder.InternalTagName = internalTagText;
            tagBuilder.StandardTagname = ContainsInvalidValues(standardTagText) ? "" : standardTagText;
            tagBuilder.TemplateTag = ContainsInvalidValues(standardTagText) ? "" : templateTagText;
            return tagBuilder.BuildTag();
        }
        private bool ContainsInvalidValues(string fieldValue) {
            return invalidValues.Contains(fieldValue.ToLower());
        }
    }
}