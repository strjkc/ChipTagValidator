using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    internal class ChipDataParser
    {
        //Constant lengths according to the specification num of bytes * 2
        private const int lenOfMacIdData = 12 * 2;
        private const int lenOfEndDelimiter = 5 * 2;
        private const int lenOfChipHeader = 27 * 2;
        List<string> validTags;

        public ChipDataParser(List<string> validTags)
        {
            this.validTags = validTags;
        }

        public List<List<Tag>> ParseChipDataStrings(List<string> chipDatastrings)
        {
            List<List<Tag>> fileCardTags = new List<List<Tag>>();
            foreach (string chipDatastring in chipDatastrings)
            {
                List<Tag> tagsInCard = new List<Tag>();
                string sanitiziedstring = RemoveHeader(chipDatastring);
                int i = 0;
                while (i < sanitiziedstring.Length)
                {
                    string tagOf2 = sanitiziedstring.Substring(i, 2);
                    string tagOf4 = sanitiziedstring.Substring(i, 4);
                    if (IsTagValid(tagOf2))
                    {
                        Tag tag = ParseTagFromString(tagOf2, i, sanitiziedstring);
                        tagsInCard.Add(tag);
                        i += GetNewPosition(tag);
                    }
                    else if (IsTagValid(tagOf4))
                    {
                        Tag tag = ParseTagFromString(tagOf4, i, sanitiziedstring);
                        tagsInCard.Add(tag);
                        i += GetNewPosition(tag);
                    }
                    else
                    {
                        i++;
                        Console.WriteLine($"Tag {tagOf2} or {tagOf4} is invalid");
                        throw new Exception("Unexpected tag");
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

        private int GetNewPosition(Tag tag)
        {
            return tag.Value.Length + tag.Name.Length + tag.Length.Length;
        }

        private Tag ParseTagFromString(string currentTagname, int currentIndex, string chipDataString)
        {

            int startOfTagLength = currentIndex + currentTagname.Length;
            int endOfTagLength = 2;
            int startOfTagValue = startOfTagLength + 2;
            string tagLength = chipDataString.Substring(startOfTagLength, endOfTagLength);
            int valueLength = Int32.Parse(tagLength, System.Globalization.NumberStyles.HexNumber) * 2;
            string tagValue = chipDataString.Substring(startOfTagValue, valueLength);
            return new Tag(currentTagname, tagLength, tagValue, "");
        }

        private Boolean IsTagValid(string tag)
        {
            foreach (string validTag in validTags)
            {
                if (validTag.Equals(tag))
                    return true;
            }
            return false;

        }

        private string RemoveHeader(string chipstring)
        {
            string removedHeader = chipstring.Substring(lenOfChipHeader);
            int lengthOfTrailer = lenOfMacIdData + lenOfEndDelimiter;
            int lengthWithoutTrailer = removedHeader.Length - lengthOfTrailer;
            return removedHeader.Substring(0, lengthWithoutTrailer);
        }
    }
}
