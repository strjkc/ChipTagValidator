using ChipTagValidator;
using System.Diagnostics;
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
                var tagsInCard = ParseEmbossData(chipDatastring);
                fileCardTags.Add(tagsInCard);
            }
            return fileCardTags;
        }

        private List<TagModel> ParseEmbossData(string chipDatastring)
        {
            List<TagModel> tagsInCard = new List<TagModel>();
            //sanitization should not be a mandatory step
            int i = 0;
            while (i < chipDatastring.Length)
            {
                string tagOf2 = chipDatastring.Substring(i, 2);
                string tagOf4 = chipDatastring.Substring(i, 4);
                //instead of isValidTag i can use IndexOf and check if index is >= 0
                TagModel validTag = IsTagValid(tagOf2) ?? IsTagValid(tagOf4);
                if (validTag != null)
                {
                    TagModel tag = ParseTagFromString(i, chipDatastring, validTag);
                    tagsInCard.Add(tag);
                    i += GetNewPosition(tag);
                }
                else
                {
                    Debug.WriteLine($"tags: {tagOf2} and {tagOf4} index: {i}");
                    throw new InvalidOperationException("Emboss file contains an invalid tag");
                }
            }

            return tagsInCard;
        }

        private int GetNewPosition(TagModel tag)
        {
            return tag.Value.Length + tag.InternalTagName.Length + tag.Length.Length;
        }

        //tag type cless/contact should be determined before creating a tag object
        private TagModel ParseTagFromString(int currentIndex, string chipDataString, TagModel validTag)
        {
            //this is safe because the name of the tag already matched the InternalTagName
            int tagNameLength = validTag.InternalTagName.Length;
            int startOfTagLength = currentIndex + tagNameLength;
            int lengthOfTagLength = 2;
            int startOfTagValue = startOfTagLength + lengthOfTagLength;
            
            string tagLengthHex = chipDataString.Substring(startOfTagLength, lengthOfTagLength);
            int tagLengthDec = Int32.Parse(tagLengthHex, System.Globalization.NumberStyles.HexNumber) * 2;
            string tagValue = chipDataString.Substring(startOfTagValue, tagLengthDec);
            TagBuilder tagBuilder = new TagBuilder().Copy(validTag);
            tagBuilder.Length = tagLengthHex;
            tagBuilder.Value = tagValue;
            return tagBuilder.BuildTag();
        }

        private TagModel IsTagValid(string tag)
        {
            foreach (TagModel validTag in validTags)
            {
                if (validTag.InternalTagName.Equals(tag))
                    return validTag;
            }
            return null;
        }

        public string RemoveHeader(string chipstring)
        {
            string removedHeader = chipstring.Substring(lenOfChipHeader);
            int lengthOfTrailer = lenOfMacIdData + lenOfEndDelimiter;
            int lengthWithoutTrailer = removedHeader.Length - lengthOfTrailer;
            return removedHeader.Substring(0, lengthWithoutTrailer);
        }
    }
}
