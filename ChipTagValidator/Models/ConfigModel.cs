using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class ConfigModel
    {
        public WordSpecParserConfig WordSpecParserConfig { get; set; }
        public VisaXmlParserConfig VisaXmlParserConfig { get; set; }
        public MastercardXmlParserConfig MastercardXmlParserConfig { get; set; }
        public ReportPrinterConfig ReportPrinterConfig { get; set; }
        public ChipDataParserConfig_inBytes ChipDataParserConfig_inBytes { get; set; }
        public BinaryParserConfig BinaryParserConfig { get; set; }
    }
}
