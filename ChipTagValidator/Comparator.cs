using DocumentFormat.OpenXml.Spreadsheet;
using TagsParser.Classes;

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
        foreach (TagModel formTag in formTags)
        {
            TagModel tagFound = formTag.IsCless
                ? embossTags.FirstOrDefault(embossTag => formTag.StandardTagname == embossTag.StandardTagname)
                : embossTags.FirstOrDefault(embossTag => formTag.StandardTagname == embossTag.InternalTagName);

            if (tagFound != null)
            {
                if (tagFound.Value != formTag.Value)
                {
                    List<TagModel> tagList = new List<TagModel>();
                    tagList.Add(formTag);
                    tagList.Add(tagFound);
                    Card.MissmatchInValues.Add(tagList);
                }
            }
            else
            {
                Card.FormTagsMissing.Add(formTag);
            }
        }
    }

    public void CheckForDuplicates(List<TagModel> embossTags, CardModel Card)
    {
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