using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTagValidator.Models;

namespace ChipTagValidator.Interfaces
{
    public interface ICacher
    {
        public void CreateCache(List<TagModel> validTags);
        public List<TagModel> LoadCache();

    }
}
