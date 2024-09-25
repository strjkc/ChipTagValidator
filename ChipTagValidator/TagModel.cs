using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    public class TagModel : ICloneable
    {
        public string StandardTagname { get; private set; }
        public string InternalTagName { get; private set; }
        public string Length { get; private set; }
        public string Value { get; private set; }
        public string TemplateTag { get; private set; }
        public bool IsCless { get; private set; }
        public bool HasTemplateTag { get; private set; }
        public bool IsMandatory { get; private set; }


        public TagModel(string standardTagName, string internalTagName, string length, string value, string templateTag, bool isCless, bool hasTemplateTag, bool isMandatory)
        {
            StandardTagname = standardTagName;
            InternalTagName = internalTagName;
            Length = length;
            Value = value;
            TemplateTag = templateTag;
            IsCless = isCless;
            HasTemplateTag = hasTemplateTag;
            IsMandatory = isMandatory;
        }

        public TagModel Clone()
        {
            return (TagModel) this.MemberwiseClone();
        }
    }
}
