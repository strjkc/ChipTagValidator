using ChipTagValidator;
using ChipTagValidator.Models;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace TagsParser.Classes
{
    public class ChipDataParser
    {
        //Constant lengths according to the specification num of bytes * 2
        private  int lenOfMacIdData = 12 * 2;
        private  int lenOfEndDelimiter = 5 * 2;
        private  int lenOfChipHeader = 27 * 2;
        List<TagModel> validTags;
        /*
        public ChipDataParser(int lenOfMacIdData, int lenOfEndDelimiter, int lenOfChipHeader) {
            _lenOfMacIdData = lenOfMacIdData;
            _lenOfEndDelimiter = lenOfEndDelimiter;
            _lenOfChipHeader = lenOfChipHeader;
        }
        */
        public ChipDataParser(List<TagModel> validTags)
        {
            this.validTags = validTags;
        }

        public List<List<TagModel>> ParseChipDataStrings(List<string> chipDatastrings)
        {
            List<List<TagModel>> fileCardTags = new List<List<TagModel>>();
            foreach (string chipDatastring in chipDatastrings)
            {
                List<TagModel> tagsInCard = ParseEmbossData(RemoveHeader(chipDatastring));
                fileCardTags.Add(tagsInCard);
            }
            return fileCardTags;
        }

        private List<TagModel> ParseEmbossData(string chipDatastring)
        {
            Log.Information($"Parsing chip data string {chipDatastring}");
            List<TagModel> tagsInCard = new List<TagModel>();
            int i = 0;
            while (i < chipDatastring.Length)
            {
                string tagOf2 = chipDatastring.Substring(i, 2);
                string tagOf4 = chipDatastring.Substring(i, 4);
                TagModel validTag = IsTagValid(tagOf2) ?? IsTagValid(tagOf4);
                if (validTag != null)
                {
                    Log.Debug($"Found Valid Tag {validTag.InternalTagName}");
                    TagBuilder tagToBuild = ParseTagFromString(i, chipDatastring, validTag);
                    TagModel tag = tagToBuild.BuildTag();
                    Log.Debug($"Parsed TLV value: {tag.InternalTagName} {tag.Length} {tag.Value}");
                    tagsInCard.Add(tag);
                    i += GetNewPosition(tag);
                }
                else
                {
                    Log.Error("Emboss file contains an invalid tag!");
                    Log.Error($"tags: {tagOf2} and {tagOf4} at position {i} in chip data string");
                    throw new InvalidOperationException("Emboss file contains an invalid tag");
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (TagModel tag in tagsInCard)
            {
                sb.Append($"{tag.InternalTagName} {tag.StandardTagname} {tag.TemplateTag} {tag.Length} {tag.Value}, ");  
            }
            Log.Information($"Parsed values of current string: {sb.ToString()}");
            return tagsInCard;
        }
        

        private int GetNewPosition(TagModel tag)
        {
            return tag.Value.Length + tag.InternalTagName.Length + tag.Length.Length;
        }

        //tag type cless/contact should be determined before creating a tag object
        private TagBuilder ParseTagFromString(int currentIndex, string chipDataString, TagModel validTag)
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

            return tagBuilder;
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
            Log.Information("Removing header and trailer from the chip data block");
            string removedHeader = chipstring.Substring(lenOfChipHeader);
            Log.Debug($"Removed header form chip data string: {chipstring.Substring(0, lenOfChipHeader)}");
            int lengthOfTrailer = lenOfMacIdData + lenOfEndDelimiter;
            int lengthWithoutTrailer = removedHeader.Length - lengthOfTrailer;
            Log.Debug($"Removed Trailer form chip data string: {removedHeader.Substring(removedHeader.Length - lengthOfTrailer)}");
            string chipDataString = removedHeader.Substring(0, lengthWithoutTrailer);
            Log.Debug($"Returning chip data string without header and trailer: {chipDataString}");
            return chipDataString;
        }
    }
}
