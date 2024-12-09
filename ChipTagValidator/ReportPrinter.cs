using ChipTagValidator.Interfaces;
using ChipTagValidator.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator
{
    public class ReportPrinter : IReportPrinter
    {
        private string _reportSufix;
        private string _missmatchTagsTitle;
        private string _duplicateTagsTitle;
        private string _missingTagsTitle;
        private string _reportLocation;
        private string _lineEndCharacters;


        public ReportPrinter() {
            ReportPrinterConfig config = Configuration.Config.ConfigModel.ReportPrinterConfig;
            _reportSufix = config.ReportSufix;
            _missmatchTagsTitle = config.MissmatchTagsTitle;
            _duplicateTagsTitle = config.DuplicateTagsTitle;
            _missingTagsTitle = config.MissingTagsTitle;
            _reportLocation = config.ReportLocation;
            _lineEndCharacters = config.LineEndCharacters;
        
        }


        private void EndLine(StreamWriter writer)
        {
            writer.WriteLine(_lineEndCharacters);
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
            if (!Directory.Exists(_reportLocation))
                Directory.CreateDirectory(_reportLocation);
            string reportPath = _reportLocation + reportName + _reportSufix;
            using (StreamWriter writer = new StreamWriter(reportPath))
            {
                Log.Information($"Creating Report: {reportPath}");
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
