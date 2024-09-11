using ChipTagValidator;

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
            MastercardXmlParser parser = new MastercardXmlParser();
            parser.Parse("C:\\Users\\Strahinja\\Downloads\\dummy.xml");
        }
    }
}
