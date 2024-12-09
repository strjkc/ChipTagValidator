using ChipTagValidator;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.CodeDom.Compiler;
using TagsParser.Classes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers;
using Serilog.Enrichers.WithCaller;
using ChipTagValidator.Models;
using DocumentFormat.OpenXml.Bibliography;
using Serilog.Sinks.RichTextBoxForms.Themes;
using Serilog.Core;
using ChipTagValidator.Interfaces;


namespace ChipTagValidatorUI
{
    public partial class formWindow : Form
    {
        LoggingLevelSwitch debugSwitch = new LoggingLevelSwitch { MinimumLevel = Serilog.Events.LogEventLevel.Information };
        //TODO refactor logging to methods
        public formWindow()
        {
            InitializeComponent();
        }

        private void embossFileButton_Click(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Display the selected file path in the TextBox
                embossFileTextBox.Text = openFileDialog.FileName;
            }
        }

        private void chipFormButton_Click(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Display the selected file path in the TextBox
                chipFormTextBox.Text = openFileDialog.FileName;
            }
        }

        private void specificationButton_Click(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a File",
                Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*|Word Files (*.doc)|*.doc"
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Display the selected file path in the TextBox
                specificationTextBox.Text = openFileDialog.FileName;
            }
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            WordSpecParser sp = new WordSpecParser();
            List<TagModel> validTags = new List<TagModel>();
            ICacher cacher = new ValidTagCacher();
            Comparator comp = new Comparator();
            IReportPrinter reportPrinter = new ReportPrinter();
            List<CardModel> cards = new List<CardModel>();

            try
            {
                Log.Information("Parse method running");

                if (specificationTextBox.Text != "")
                {
                    validTags = sp.Parse(specificationTextBox.Text);
                    cacher.CreateCache(validTags);
                }
                else
                {
                    validTags = cacher.LoadCache();
                }

                if (validTags.Count == 0)
                {
                    throw new InvalidDataException("No valid tags available. Provide specification or data file");
                }

                if (chipDataDelimiterTextBox.Text.Length < 1)
                {
                    throw new InvalidDataException("Chip data delimiter must be provided, check your emboss specification");
                }

                IBinaryParser binParser = new BinaryParser(chipDataDelimiterTextBox.Text);
                List<string> parsedCards = binParser.Parse(embossFileTextBox.Text);
                ChipDataParser cp = new ChipDataParser(validTags);
                List<List<TagModel>> tml = cp.ParseChipDataStrings(parsedCards);
                IChipFormParserFactory chipParserFactory;
                switch (brandComboBox.Text)
                {
                    case "Visa":
                        chipParserFactory = new VisaChipParserFactory();
                        break;
                    case "Mastercard":
                        chipParserFactory = new McChipParserFactory();
                        break;
                    default:
                        throw new InvalidDataException("Parser not available for selected card brand");
                }
                IChipFileParser chipFormParser;
                string x = stripFileType(chipFormTextBox.Text);
                switch (stripFileType(chipFormTextBox.Text)) {
                    case "xml":
                        chipFormParser =  chipParserFactory.CreateXmlParser(); 
                        break;
                    default: throw new InvalidDataException($"Parser not available for selected file type: {stripFileType(chipFormTextBox.Text)}");
                }

                List<TagModel> vpaTags = chipFormParser.Parse(chipFormTextBox.Text);

                foreach (List<TagModel> list in tml)
                {
                    CardModel card = new CardModel();
                    //       comp.Compare(list, vpaTags, card);
                    card.AllChipData = list;
                    card.PAN = list.FirstOrDefault(tag => tag.InternalTagName == "5A").Value;
                    cards.Add(card);
                }
                reportPrinter.WriteReport(cards, stripFileName(embossFileTextBox.Text));
            }
            catch (ArgumentException ex)
            {
                Log.Error("Emboss file field can not be empty!");
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message} {ex.StackTrace}");
            }

        }

        private string stripFileName(string fileName)
        {
            int indexOfBackslash = fileName.LastIndexOf("\\");
            string fileNameFull = fileName.Substring(indexOfBackslash);
            int indexOfDot = fileNameFull.IndexOf(".");
            return fileNameFull.Substring(0, indexOfDot);
        }

        private string stripFileType(string path)
        {
            int indexOfDot = path.LastIndexOf(".");
            return path.Substring(indexOfDot + 1);
        }




        private void formWindow_Load(object sender, EventArgs e)
        {
            var log = new LoggerConfiguration()
                .Enrich.WithCaller()
                .MinimumLevel.ControlledBy(debugSwitch)
                .WriteTo.RichTextBox(logTextBox, outputTemplate: "[{Timestamp:yyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}", theme: ThemePresets.Light, messageBatchSize: 1, messagePendingInterval: 1, autoScroll: true, maxLogLines: 10000, minimumLogEventLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("log.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}", flushToDiskInterval: TimeSpan.FromMilliseconds(1000))
                .CreateLogger();
            Log.Logger = log;
        }

        private void debugLogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            debugSwitch.MinimumLevel = debugLogCheckbox.Checked ? Serilog.Events.LogEventLevel.Debug : Serilog.Events.LogEventLevel.Information;
            if (debugLogCheckbox.Checked)
            {
                Log.Information("--- Debug log available in debug_log.txt file in the root directory of the application ---");
            }

        }

    }
}
