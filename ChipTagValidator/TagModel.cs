using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    public class TagModel
    {
        private String name;
        public String length;
        private String value;
        private String templateTag;

        public String Name { get; private set; }
        public String Length { get; private set; }
        public String Value { get; private set; }
        public String TemplateTag { get; private set; }

        public TagModel(string name, string length, string value, string templateTag)
        {
            Name = name;
            Length = length;
            Value = value;
            TemplateTag = templateTag;
        }
    }
}
