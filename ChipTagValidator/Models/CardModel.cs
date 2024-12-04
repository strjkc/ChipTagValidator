using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Models
{
    public class CardModel
    {
        public string PAN { get; set; }
        public List<TagModel> AllChipData { get; set; }
        public List<TagModel> Duplicates { get; set; } = new List<TagModel>();

        //The first card in the array is allways the Tag from the Chip Form, the second is tag from the Emboss file
        public List<List<TagModel>> MissmatchInValues { get; set; } = new List<List<TagModel>>();
        public List<TagModel> FormTagsMissing { get; set; } = new List<TagModel>();
        public List<TagModel> MandatoryTagsMissing { get; set; } = new List<TagModel>();

    }
}
