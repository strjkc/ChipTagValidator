using ChipTagValidator.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Serilog;

namespace ChipTagValidator;

public class Comparator
{
    /*
     *vpa tag mi ima standardtag, value, length
     * emboss tag mi ima internal, standard, value, length
     *
     * 
     * trebaju mi nekoliko globalnih lista
     * Duplicates
     * mandatory tags missing
     * form tags missing
     * mismatch in values
     * 
     * prolazim kroz listu tagova u listi vpa tagova koju sam dobio od parsiranja forme
     * taj tag sadrzi standardTag, length i value
     * za svaki tag kazem prodji kroz listu iz embosa i ako je tag cless onda pitaj da li je standardtag == standardtagu iz druge forme ako jeste uporedi vrednosti
     * ako nije i dosli smo do kraja dodaj u listu
     */


    //TODO: List of non mandatory non vpa tags in emboss file
    public void Compare(List<TagModel> embossTags, List<TagModel> formTags, CardModel Card)
    {
        Log.Information("Comparing Chip form tags to emboss file");
        foreach (TagModel formTag in formTags)
        {
            Log.Debug($"Tag from Chip form {formTag.StandardTagname} isCless: {formTag.IsCless}");
            /*  
             * standard tag name - internaly used for contact tags, ICOs use them for contact and cless
             * internal tag name - the name of the replacetement tag used for clontactless tags
             * if we are searching for a cless tag then internal tag name that we use will be different because the standard
             * tag name is used for the contact tag.
            */
            TagModel tagFound = formTag.IsCless
                ? embossTags.FirstOrDefault(embossTag => formTag.StandardTagname == embossTag.StandardTagname && embossTag.IsCless)
                : embossTags.FirstOrDefault(embossTag => formTag.StandardTagname == embossTag.InternalTagName);

            if (tagFound != null)
            {

                Log.Debug($"Tag {tagFound.InternalTagName}/{tagFound.StandardTagname} found, comparing values");

                if (tagFound.Value != formTag.Value)
                {

                    Log.Debug($"Missmatch in values for tags ${tagFound.InternalTagName}/{tagFound.StandardTagname} value {tagFound.Value} and {formTag.StandardTagname} value {formTag.Value}");
                    List<TagModel> tagList = new List<TagModel>();
                    tagList.Add(formTag);
                    tagList.Add(tagFound);
                    Card.MissmatchInValues.Add(tagList);
                }
            }
            else
            {
                Log.Debug("Tag not found");
                Card.FormTagsMissing.Add(formTag);
            }
        }
    }

    public void CheckForDuplicates(List<TagModel> embossTags, CardModel Card)
    {
        Log.Information("Checking for duplicates");
        HashSet<TagModel> seen = new HashSet<TagModel>();
        foreach (TagModel embossTag in embossTags)
        {
            if (!seen.Add(embossTag))
            {
                Card.Duplicates.Add(embossTag);
            }
        }
    }
}