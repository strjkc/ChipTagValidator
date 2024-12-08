using ChipTagValidator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator
{
    public class McChipParserFactory : IChipFormParserFactory
    {
        public IChipFileParser CreateXmlParser()
        {
            return new MastercardXmlParser();
        }
    }
}
