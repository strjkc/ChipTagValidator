using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class ChipDataParserConfig_inBytes
    {
        public int LenOfMacIdData { get; set; }
        public int LenOfEndDelimiter { get; set; }
        public int LenOfChipHeader { get; set; }
    }
}
