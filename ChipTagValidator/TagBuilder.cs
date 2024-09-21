using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;

namespace ChipTagValidator
{
    internal class TagBuilder
    {



        public string StandardTagname {private get; set; }
        public string InternalTagName { private get; set; }
        public string Length { private get;  set; }
        public string Value { private get; set; }
        public string TemplateTag { private get; set; }
        public bool IsCless { private get; set; }
        public bool HasTemplateTag { private get; set; }
        public bool IsMandatory { private get; set; }


        public TagModel BuildTag() {
            return new TagModel(StandardTagname, InternalTagName, Length, Value, TemplateTag, IsCless, HasTemplateTag, IsMandatory);
        }
    }
}
