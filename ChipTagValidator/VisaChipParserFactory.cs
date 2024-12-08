using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTagValidator.Interfaces;

namespace ChipTagValidator
{
    public class VisaChipParserFactory : IChipFormParserFactory
    {
        public IChipFileParser CreateXmlParser()
        {
            return new VisaXmlParser();
        }
    }
}
