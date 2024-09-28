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
     * duplicates
     * mandatory tags missing
     * form tags missing
     * mismatch in values
     * 
     * prolazim kroz listu tagova u listi vpa tagova koju sam dobio od parsiranja forme
     * taj tag sadrzi standardTag, length i value
     * za svaki tag kazem prodji kroz listu iz embosa i ako je tag cless onda pitaj da li je standardtag == standardtagu iz druge forme ako jeste uporedi vrednosti
     * ako nije i dosli smo do kraja dodaj u listu
     */

    private List<TagModel> duplicates;
    private Dictionary<string, TagModel[]> missmatchInValues;
    private List<TagModel> formTagsMissing;
    private List<TagModel> mandatoryTagsMissing;
    private List<TagModel> embossTags;
    private List<TagModel> formTags;

    public Comparator(List<TagModel> formTags, List<TagModel> embossTags)
    {
        this.embossTags = embossTags;
        this.formTags = formTags;
    }

    public void Compare()
    {
        foreach (TagModel formTag in formTags)
        {
            bool tagIsFound = false;
            foreach (TagModel embossTag in embossTags)
            {
                //if the tag is contactless I want to compare the standard tag names
                if (formTag.IsCless)
                {
                    if (formTag.StandardTagname == embossTag.StandardTagname)
                    {
                        if (formTag.Value != embossTag.Value)
                        {
                            missmatchInValues.Add(formTag.StandardTagname, new TagModel[]{formTag, embossTag});
                        }
                        tagIsFound = true;
                    }
                }
                else
                {
                    if (formTag.StandardTagname == embossTag.InternalTagName)
                    {
                        if (formTag.Value != embossTag.Value)
                        {
                            missmatchInValues.Add(formTag.StandardTagname, new TagModel[]{formTag, embossTag});
                        }
                        tagIsFound = true;
                    }
                }
                if (!tagIsFound)
                {
                    formTagsMissing.Add(formTag);
                }
            }
        }
        CheckForDuplicates();
    }

    private void CheckForDuplicates()
    {
        HashSet<TagModel> seen = new HashSet<TagModel>();
        foreach (TagModel embossTag in embossTags)
        {
            if (!seen.Add(embossTag))
            {
                duplicates.Add(embossTag);
            }
        }
    }
}