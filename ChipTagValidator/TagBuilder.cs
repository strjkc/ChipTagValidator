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



        public string StandardTagname { get; set; }
        public string InternalTagName {  get; set; }
        public string Length {  get;  set; }
        public string Value {  get; set; }
        public string TemplateTag {  get; set; }
        public bool IsCless {  get; set; }
        public bool HasTemplateTag {  get; set; }
        public bool IsMandatory {  get; set; }


        public TagModel BuildTag() {
            return new TagModel(StandardTagname, InternalTagName, Length, Value, TemplateTag, IsCless, HasTemplateTag, IsMandatory);
        }

        public TagBuilder()
        { }

        public TagBuilder Copy(TagModel tag)
        {
            StandardTagname = tag.StandardTagname;
            InternalTagName = tag.InternalTagName;
            Length = tag.Length;
            Value = tag.Value;
            TemplateTag = tag.TemplateTag;
            IsCless = tag.IsCless;
            HasTemplateTag = tag.HasTemplateTag;
            IsMandatory = tag.IsMandatory;
            return this;
        }
    }
}
