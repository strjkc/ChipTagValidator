using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    public class TagModel
    {
        public string Name { get; private set; }
        public string Length { get; private set; }
        public string Value { get; private set; }
        public string TemplateTag { get; private set; }

        public TagModel(string name, string length, string value, string templateTag)
        {
            Name = name;
            Length = length;
            Value = value;
            TemplateTag = templateTag;
        }
    }
}
