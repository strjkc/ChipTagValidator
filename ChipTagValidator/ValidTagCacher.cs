using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTagValidator.Interfaces;
using ChipTagValidator.Models;
using Serilog;

namespace ChipTagValidator
{
    public class ValidTagCacher : ICacher
    {
        public void CreateCache(List<TagModel> validTags)
        {            
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TagModel tag in validTags)
            {
                stringBuilder.Append($"StandardTagname: {tag.StandardTagname} InternalTagName: {tag.InternalTagName} TemplateTag: {tag.TemplateTag}, ");
            }
            Log.Debug($"Tags to cache: {stringBuilder.ToString()}");
            
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
            Log.Information("Loading valid tags from cache");
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
            StringBuilder sb = new StringBuilder();
            foreach (TagModel tag in validTags)
            {
                sb.Append($"StandardTagname: {tag.StandardTagname} InternalTagName: {tag.InternalTagName} TemplateTag: {tag.TemplateTag}, ");
            }
            Log.Information($"Loaded folowing valid tags from cache: {sb.ToString()}");
            return validTags;

        }
    }
}
