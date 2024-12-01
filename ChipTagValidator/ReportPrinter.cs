using ChipTagValidator.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public class ReportPrinter : IReportPrinter
    {
        private string _reportSufix = "_parsed.txt";
        private const string _missmatchTagsTitle = "#### Missmatch in values: #####\n";
        private const string _duplicateTagsTitle = "#### Duplicate Tags: ####\n";
        private const string _missingTagsTitle = "#### Tags Missing From Chip Form: ####\n";
        private string _reportLocation = ".\\";
        private string lineEndCharacters = "";

        private void EndLine(StreamWriter writer)
        {
            writer.WriteLine(lineEndCharacters);
        }
        private void LineWriter(StreamWriter writer, string tag, string length, string value)
        {
            //used to allign all tlv columns
            string variableSpacing = tag.Length == 2 ? "\t\t" : "\t";
            writer.WriteLine(tag + variableSpacing + length + "\t\t" + value);
        }

        private void WriteTags(List<TagModel> tags, string title, StreamWriter writer) {
            writer.WriteLine(title);
            if (tags.Count > 0)
                foreach (TagModel tag in tags)
                {
                    LineWriter(writer, tag.InternalTagName ?? tag.StandardTagname, tag.Length, tag.Value);
                }
            else
                writer.WriteLine("None");
            EndLine(writer);
        }

        public void WriteReport(List<CardModel> cards, string reportName) { 
            using(StreamWriter writer = new StreamWriter(_reportLocation+reportName+_reportSufix))
            {
                foreach(CardModel card in cards)
                {
                    WriteTags(card.AllChipData, "Details for card " + card.PAN + " :\n", writer);
                    WriteTags(card.Duplicates, _duplicateTagsTitle, writer);
                    WriteTags(card.FormTagsMissing, _missingTagsTitle, writer);

                    writer.WriteLine(_missmatchTagsTitle);
                    foreach (List<TagModel> tagList in card.MissmatchInValues)
                    {
                        writer.WriteLine("Tag from Chip form: ");
                        LineWriter(writer, tagList[0].StandardTagname, tagList[0].Length, tagList[0].Value);
                        writer.WriteLine("Tag from Emboss file: ");
                        LineWriter(writer, tagList[1].InternalTagName, tagList[1].Length, tagList[1].Value);
                        writer.WriteLine("");
                    }
                    EndLine(writer);
                    writer.WriteLine("#### END OF CARD ####\n");
                }
                writer.Flush();
            }
        }
    }
}
