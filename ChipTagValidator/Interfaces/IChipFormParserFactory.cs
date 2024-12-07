using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Interfaces
{
    public interface IChipFormParserFactory
    {

        public XmlParser CreateXmlParser();
        /*
          future implementation
        
        HtmlParser CreateHtmlParser();
        PdfParser CreatePdfParser();
        */



    }
}
