using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class MastercardXmlParserConfig
    {
        public string RootTag { get; set; }
        public string TagDescription { get; set; }
        public string TagName { get; set; }
        public string TagValue { get; set; }
    }
}
