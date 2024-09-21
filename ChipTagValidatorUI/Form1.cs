using ChipTagValidator;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.CodeDom.Compiler;
using TagsParser.Classes;

namespace ChipTagValidatorUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            List<TagModel> validTags = [new TagModel("4F", "", "", "", "", false), new TagModel("8E", "", "", "", "", false) , new TagModel("8E", "DF4B", "", "", "", true)];
            IParser binaryParser = new BinaryParser();
            ChipDataParser cp = new ChipDataParser(validTags);
            cp.ParseChipDataStrings(binaryParser.Parse("C:\\Users\\Strahinja\\Downloads\\abiCvbd231122001.txt"));
            */
            WordSpecParser sp = new WordSpecParser();
            BinaryParser binParser = new BinaryParser();
            try
            {

                List<TagModel> validTags = sp.Parse(@"C:\Users\Strahinja\Desktop\Embossing_Spec_Strahinja.docx");
                List<string> parsedCards = binParser.Parse(@"C:\Users\Strahinja\Downloads\abiCvbd231122001.txt");
                ChipDataParser cp = new ChipDataParser(validTags);
                List<List<TagModel>> tml =  cp.ParseChipDataStrings(parsedCards);

                XmlParser xmlParser;
                

            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
