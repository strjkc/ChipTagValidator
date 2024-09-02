using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    internal class CardModel
    {
        public String PAN { get; set; }
        public List<TagModel> AllChipData { get; set; }
        public List<TagModel> MissingData { get; set; }
        public List<TagModel> ExtraData { get; set; }
        public List<TagModel> Mismatch { get; set; }

    }
}
