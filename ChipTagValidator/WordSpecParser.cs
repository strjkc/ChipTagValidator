using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;
using ChipTagValidator.Models;
using Serilog;

namespace ChipTagValidator
{
    public class WordSpecParser
    {
        //TODO: these should be extracted to parameter values
        WordSpecParserConfig config;
        private int expectedColumnCount;
        private int startFromTable;
        private string[] invalidValues;
        int columnForInternalTags;
        int columnForStandardTags;
        int columnForTemplateTags;
        int columnForMandatoryTags;


        public WordSpecParser() {
            config = Configuration.Config.ConfigModel.WordSpecParserConfig;
            expectedColumnCount = config.ExpectedColumnCount;
            startFromTable = config.StartFromTable;
            invalidValues = config.InvalidValues;
            columnForInternalTags = config.ColumnForInternalTags;
            columnForStandardTags = config.ColumnForStandardTags;
            columnForTemplateTags = config.ColumnForTemplateTags;
            columnForMandatoryTags = config.ColumnForMandatoryTags;
        }

        public List<TagModel> Parse(string file) {
            Log.Information($"Parsing Emboss file specification: {file}");
            List<TagModel> tagModels = new List<TagModel>();

            using WordprocessingDocument doc = WordprocessingDocument.Open(file, false) 
                ?? throw new FileNotFoundException("Error on opening the Word Document");

            // The doc can containt a lot of tables, start from the first table relevant to chip data
            Log.Debug($"Starting from table {startFromTable}");
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
                Log.Debug($"internalTagText {internalTagText}, standardTagText {standardTagText}, templateTagText{templateTagText}");
                
                tagBuilder.InternalTagName = internalTagText;
                tagBuilder.StandardTagname = ContainsInvalidValues(standardTagText) ? internalTagText : standardTagText;
                tagBuilder.TemplateTag = ContainsInvalidValues(templateTagText) ? "" : templateTagText;
                tagBuilder.IsCless = ContainsInvalidValues(standardTagText) ? false : true;
                Log.Debug($"tag built: internal tag name {tagBuilder.InternalTagName}, standard tag name {tagBuilder.StandardTagname}, template tag name {tagBuilder.TemplateTag}");
            }
            catch (Exception e) {
                Log.Debug($"Column count : {row.ChildElements.Count}, indexes provided: {columnForInternalTags}, {columnForStandardTags}, {columnForTemplateTags}");
                throw new IndexOutOfRangeException("Unable to retreive values from clumns under given indexes");
            }
            return tagBuilder.BuildTag();
        }

        private bool IsTableHeader(TableRow row) {
            Log.Debug($" Row type: {row.ChildElements[0].GetType().Name}");
            bool isTableHeader = row.ChildElements[0].GetType().Name == "TableRowProperties";
            if (isTableHeader)
                Log.Debug("Table header identified, skipping");
            else
                Log.Debug($"Row is not header, first child is {row.ChildElements[0].InnerText}");
            return isTableHeader;
        }

        private bool ContainsInvalidValues(string fieldValue) {
            return invalidValues.Contains(fieldValue.ToLower());
        }
    }
}