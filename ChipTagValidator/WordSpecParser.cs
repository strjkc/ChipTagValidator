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
        private const int expectedColumnCount = 6;
        private const int startFromTable = 5;
        private string[] invalidValues = { "n/a", "/", "", " "};
        int columnForInternalTags = 1;
        int columnForStandardTags = 3;
        int columnForTemplateTags = 4;
        int columnForMandatoryTags = 5;

        public List<TagModel> Parse(string file) {
            List<TagModel> tagModels = new List<TagModel>();

            using WordprocessingDocument doc = WordprocessingDocument.Open(file, false) 
                ?? throw new FileNotFoundException("Error on opening the Word Document");
            
            // The doc can containt a lot of tables, start from the first table relevant to chip data

            var tables = doc.MainDocumentPart.Document.Body.Elements<Table>().Skip(startFromTable).ToList()
                        ?? throw new NullReferenceException("Tables not found in document");
            foreach (Table table in tables)
            {
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    // Presence of "TableProperties" indicates a table header, don't parse the header,
                    // also if the expected number of columns is not equal to the number of columns in row, don't parse it
                    if (!IsTableHeader(row) && (row.ChildElements.Count == expectedColumnCount))
                    {
                        TagModel tag = BuildTagFromData(row);
                        tagModels.Add(tag);
                    }
                }
                }
            return tagModels;

        }


        private TagModel BuildTagFromData(TableRow row) {
            var tagBuilder = new TagBuilder();
            // TODO: add mandatory field
            try
            {
                string internalTagText = row.ChildElements[columnForInternalTags].InnerText.Trim();
                string standardTagText = row.ChildElements[columnForStandardTags].InnerText.Trim();
                string templateTagText = row.ChildElements[columnForTemplateTags].InnerText.Trim();

                tagBuilder.InternalTagName = internalTagText;
                tagBuilder.StandardTagname = ContainsInvalidValues(standardTagText) ? "" : standardTagText;
                tagBuilder.TemplateTag = ContainsInvalidValues(templateTagText) ? "" : templateTagText;
            }
            catch (Exception e) {
                throw new IndexOutOfRangeException("Unable to retreive values from clumns under given indexes");
                Debug.WriteLine($"Column count : {row.ChildElements.Count}, indexes provided: {columnForInternalTags}, {columnForStandardTags}, {columnForTemplateTags}");
            }
            return tagBuilder.BuildTag();
        }

        private bool IsTableHeader(TableRow row) {
            bool isTableHeader = row.ChildElements[0].GetType().Name == "TableRowProperties";
            if (isTableHeader)
                Debug.WriteLine("Table header identified, skipping");
            else
                Debug.WriteLine($"Row is not header, first child is {row.ChildElements[0].InnerText}");
            return isTableHeader;
        }

        private bool ContainsInvalidValues(string fieldValue) {
            return invalidValues.Contains(fieldValue.ToLower());
        }
    }
}