using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class VisaXmlParserConfig
    {
        public string Tagelement { get; set; }
        public string LengthElement { get; set; }       
        public string ValueElement { get; set; }        
        public string TypeElement { get; set; }
        public string Attribute { get; set; }
    }
}
