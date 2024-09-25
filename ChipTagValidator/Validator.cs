using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;

namespace ChipTagValidator
{
    internal class Validator
    {

        public List<TagModel> FormTags { get; set; }
        public List<TagModel> EmbossTags { get; set; }
        public List<TagModel> MissingTags { get; set; }
        public List<TagModel> DuplicateTags { get; set; }
        public List<List<TagModel>> InvalidValues { get; set; }


        public void Validate() {

            foreach (var formTag in FormTags) { 
                foreach(var embossTag in EmbossTags)
                {
                    if (formTag.InternalTagName == embossTag.InternalTagName || formTag.InternalTagName == embossTag.InternalTagName) ;
                }
            }
        }

    }
}
