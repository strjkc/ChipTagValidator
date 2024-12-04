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


namespace ChipTagValidatorUI
{
    public partial class formWindow : Form
    {
        LoggingLevelSwitch debugSwitch = new LoggingLevelSwitch { MinimumLevel = Serilog.Events.LogEventLevel.Information };
        //TODO Wire up inputs
        //TODO validate inputs
        //TODO refactor logging to methods
        //TODO add aditional logging
        
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
            BinaryParser binParser = new BinaryParser();
            try
            {
                Log.Information("Parse method running");

                List<TagModel> validTags = sp.Parse(@"C:\Users\Strahinja\Desktop\Embossing_Spec_Strahinja.docx");
                ValidTagCacher vt = new ValidTagCacher();
                vt.CreateCache(validTags);
                List<TagModel> validTags2 = vt.LoadCache();
                List<string> parsedCards = binParser.Parse(@"C:\Users\Strahinja\Downloads\abiCvbd231122001.txt");
                ChipDataParser cp = new ChipDataParser(validTags);
                List<List<TagModel>> tml = cp.ParseChipDataStrings(parsedCards);

                XmlParser xmlParser = new VisaXmlParser();
                List<TagModel> vpaTags = xmlParser.Parse(@"C:\Users\Strahinja\Downloads\visa.xml");
                Comparator comp = new Comparator();
                List<CardModel> cards = new List<CardModel>();
                foreach (List<TagModel> list in tml)
                {
                    CardModel card = new CardModel();
                    comp.Compare(list, vpaTags, card);
                    card.AllChipData = list;
                    card.PAN = list.FirstOrDefault(tag => tag.InternalTagName == "5A").Value;
                    cards.Add(card);
                }
                ReportPrinter reportPrinter = new ReportPrinter();
                reportPrinter.WriteReport(cards, "abiCards");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }





        private void formWindow_Load(object sender, EventArgs e)
        {
            var log = new LoggerConfiguration()
                .Enrich.WithCaller()
                .MinimumLevel.ControlledBy(debugSwitch)
                .WriteTo.RichTextBox(logTextBox, outputTemplate: "[{Timestamp:yyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}", theme: ThemePresets.Light,        messageBatchSize: 1,        messagePendingInterval: 1,        autoScroll: true,        maxLogLines: 10000, minimumLogEventLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("log.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}", flushToDiskInterval: TimeSpan.FromMilliseconds(1000))
                .CreateLogger();
            Log.Logger = log;
        }

        private void debugLogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            debugSwitch.MinimumLevel = debugLogCheckbox.Checked ? Serilog.Events.LogEventLevel.Debug : Serilog.Events.LogEventLevel.Information;
            if (debugLogCheckbox.Checked) {
                Log.Information("--- Debug log available in debug_log.txt file in the root directory of the application ---");
            }

        }
    }
}
