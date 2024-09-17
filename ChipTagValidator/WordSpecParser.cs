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
        //we expect the number of the columns in the document to be 5, and that the 5th table is containing chip data
        private const int expectedColumnCount = 5;
        private const int startFromTable = 5;

        public List<TagModel> Parse(string file) {
            int columnForInternalTags = 1;
            int columnForStandardTags = 3;
            int columnForTemplateTeags = 4;


            List<TagModel> tagModels = new List<TagModel>();
            using WordprocessingDocument doc = WordprocessingDocument.Open(file, false);
            Body body = doc.MainDocumentPart.Document.Body;
            
            
            var tables = body.Elements<Table>().ToList();
            for (int i = startFromTable; i < tables.Count; i++)
            {
                //od 5 do 12 mi treba
                var table = tables[i];
                // uzmi ako je u drugoj koloni nesto sto moze da se konvertuje u broj, onda uzmi tag
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    if(row.ChildElements.Count == expectedColumnCount) { 
                        var tag = new TagModel("", "", "", "", "", false, false);

                        string internalTagText = row.ChildElements[columnForInternalTags].InnerText.Trim();
                        string standardTagText = row.ChildElements[columnForStandardTags].InnerText.Trim();
                        string templateTagText = row.ChildElements[columnForTemplateTeags].InnerText.Trim();

                        tag.InternalTagName = internalTagText;
                        tag.StandardTagname = string.Equals(standardTagText, "n/a", StringComparison.OrdinalIgnoreCase) ? "" : standardTagText;
                        tag.TemplateTag = string.Equals(templateTagText, "n/a", StringComparison.OrdinalIgnoreCase) ? "" : templateTagText;
                        
                        tagModels.Add(tag);

                    }

                }
                }
            return tagModels;

        }
    }
}