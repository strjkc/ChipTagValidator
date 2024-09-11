using System.Text;

namespace TagsParser.Classes
{
    public class ChipDataParser
    {
        //Constant lengths according to the specification num of bytes * 2
        private const int lenOfMacIdData = 12 * 2;
        private const int lenOfEndDelimiter = 5 * 2;
        private const int lenOfChipHeader = 27 * 2;
        List<TagModel> validTags;

        public ChipDataParser(List<TagModel> validTags)
        {
            this.validTags = validTags;
        }

        public List<List<TagModel>> ParseChipDataStrings(List<string> chipDatastrings)
        {
            List<List<TagModel>> fileCardTags = new List<List<TagModel>>();
            foreach (string chipDatastring in chipDatastrings)
            {
                List<TagModel> tagsInCard = new List<TagModel>();
                //sanitization should not be a mandatory step
                string sanitiziedstring = RemoveHeader(chipDatastring);
                int i = 0;
                while (i < sanitiziedstring.Length)
                {
                    string tagOf2 = sanitiziedstring.Substring(i, 2);
                    string tagOf4 = sanitiziedstring.Substring(i, 4);
                    //instead of isValidTag i can use IndexOf and check if index is >= 0
                    TagModel validTag2 = IsTagValid(tagOf2);
                    TagModel validTag4 = IsTagValid(tagOf4);
                    if (validTag2 != null)
                    {
                        TagModel tag = ParseTagFromString(tagOf2, i, sanitiziedstring, validTag2);
                        tagsInCard.Add(tag);
                        i += GetNewPosition(tag);
                    }
                    else if (validTag4 != null)
                    {
                        TagModel tag = ParseTagFromString(tagOf4, i, sanitiziedstring, validTag4);
                        tagsInCard.Add(tag);
                        i += GetNewPosition(tag);
                    }
                    else
                    {
                        i++;
                        Console.WriteLine($"Tag {tagOf2} or {tagOf4} is invalid");
                        //throw new Exception("Unexpected tag");
                        //break;
                    }
                }
                fileCardTags.Add(tagsInCard);
            }
            return fileCardTags;
        }

        private int GetDecNumber(string hexstring)
        {
            byte[] byteArray = Encoding.Default.GetBytes(hexstring);
            return BitConverter.ToInt32(byteArray);

        }

        private int GetNewPosition(TagModel tag)
        {
            return tag.Value.Length + tag.ContactName.Length + tag.Length.Length;
        }

        //tag type cless/contact should be determined before creating a tag object
        private TagModel ParseTagFromString(string currentTagname, int currentIndex, string chipDataString, TagModel validTag)
        {

            int startOfTagLength = currentIndex + currentTagname.Length;
            int endOfTagLength = 2;
            int startOfTagValue = startOfTagLength + 2;
            string tagLength = chipDataString.Substring(startOfTagLength, endOfTagLength);
            int valueLength = Int32.Parse(tagLength, System.Globalization.NumberStyles.HexNumber) * 2;
            string tagValue = chipDataString.Substring(startOfTagValue, valueLength);
            return new TagModel(validTag.ContactName, validTag.ClessName, tagLength, tagValue, "", validTag.IsCless);
        }

        private TagModel IsTagValid(string tag)
        {
            foreach (TagModel validTag in validTags)
            {
                if (validTag.ContactName.Equals(tag) || validTag.ClessName.Equals(tag))
                    return validTag;
            }
            return null;
        }

        //ovo treba da se izvadi odavde


        private string RemoveHeader(string chipstring)
        {
            string removedHeader = chipstring.Substring(lenOfChipHeader);
            int lengthOfTrailer = lenOfMacIdData + lenOfEndDelimiter;
            int lengthWithoutTrailer = removedHeader.Length - lengthOfTrailer;
            return removedHeader.Substring(0, lengthWithoutTrailer);
        }
    }
}
