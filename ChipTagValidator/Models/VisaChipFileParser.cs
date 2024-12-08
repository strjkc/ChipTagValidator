using ChipTagValidator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class VisaChipFileParser : IChipFormParserFactory
    {
        public IChipFileParser CreateXmlParser()
        {
            return new VisaXmlParser();
        }

    }
}
