using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class WordSpecParserConfig
    {
        public int ExpectedColumnCount { get; set; }
        public int StartFromTable { get; set; }
        public string[] InvalidValues { get; set; }
        public int ColumnForInternalTags { get; set; }
        public int ColumnForStandardTags { get; set; }
        public int ColumnForTemplateTags { get; set; }
        public int ColumnForMandatoryTags { get; set; }


    }
}
