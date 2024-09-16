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
        private const int expectedColumnCount = 5;

        public List<TagModel> Parse(string file) {
            int columnForInternalTags = 1;
            int columnForStandardTags = 3;
            int columnForTemplateTeags = 4;


            List<TagModel> tagModels = new List<TagModel>();
            using WordprocessingDocument doc = WordprocessingDocument.Open(file, false);
            Body body = doc.MainDocumentPart.Document.Body;
            
            
            var tables = body.Elements<Table>().ToList();
            for (int i = 5; i < tables.Count; i++)
            {
                //od 5 do 12 mi treba
                //this should be parametarised
                var table = tables[i];
                // uzmi ako je u drugoj koloni nesto sto moze da se konvertuje u broj, onda uzmi tag
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    if(row.ChildElements.Count == expectedColumnCount) { 
                        var tag = new TagModel("", "", "", "", "", false, false);
                        if (!row.ChildElements[columnForStandardTags].InnerText.ToLower().Contains("n/a"))
                        {
                            tag.InternalTagName = row.ChildElements[columnForInternalTags].InnerText;
                            tag.StandardTagname = row.ChildElements[columnForStandardTags].InnerText;
                        }
                     }
                        
                        else
                        {
                            tag.InternalTagName = row.ChildElements[1].InnerText;
                        }


                        tagModels.Add(tag);
                    
                    }
                }
            }

            return tagModels;
        }
    }
}