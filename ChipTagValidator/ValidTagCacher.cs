using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public class ValidTagCacher : ICacher
    {
        public void CreateCache(List<TagModel> validTags)
        {
            using (StreamWriter writer = new StreamWriter("validTags.txt"))
            {
                foreach (TagModel tag in validTags)
                {
                    writer.WriteLine(tag.StandardTagname + "," + tag.InternalTagName+ "," + tag.TemplateTag);
                }
                writer.Flush();    
            }

        }

        public List<TagModel> LoadCache()
        {
            List<TagModel> validTags = new List<TagModel>();

            using(StreamReader reader = new StreamReader("validTags.txt"))
            {
                string tagString;
                while ((tagString = reader.ReadLine()) != null) {
                    TagBuilder tagBuilder = new TagBuilder();
                    string[] tagDetails = tagString.Split(",");
                    tagBuilder.StandardTagname = tagDetails[0];
                    tagBuilder.InternalTagName = tagDetails[1];
                    tagBuilder.TemplateTag = tagDetails[2];
                    validTags.Add(tagBuilder.BuildTag());
                }
            }

            return validTags;

        }
    }
}
