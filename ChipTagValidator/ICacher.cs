using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;

namespace ChipTagValidator
{
    public interface ICacher
    {
        public void CreateCache(List<TagModel> validTags);
        public List<TagModel> LoadCache();

    }
}
