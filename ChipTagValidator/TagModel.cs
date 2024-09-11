using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    public class TagModel
    {
        public string ContactName { get; private set; }
        public string ClessName { get; set; }
        public string Length { get; private set; }
        public string Value { get; private set; }
        public string TemplateTag { get; private set; }
        public bool IsCless { get; set; }

        public TagModel(string contactName, string clessName, string length, string value, string templateTag, bool isCless)
        {
            ContactName = contactName;
            ClessName = clessName;
            Length = length;
            Value = value;
            TemplateTag = templateTag;
            IsCless = isCless;
        }
    }
}
