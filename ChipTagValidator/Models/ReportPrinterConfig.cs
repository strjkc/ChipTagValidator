using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class ReportPrinterConfig
    {
        public string ReportSufix { get; set; }
        public string  MissmatchTagsTitle { get; set; }
        public string DuplicateTagsTitle { get; set; }
        public string MissingTagsTitle { get; set; }
        public string ReportLocation { get; set; }
        public string LineEndCharacters { get; set; }
    }
}
