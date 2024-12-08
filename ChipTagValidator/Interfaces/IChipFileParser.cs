using ChipTagValidator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChipTagValidator.Interfaces
{
    public interface IChipFileParser
    {
        public T LoadFile<T>(string path);
        public List<TagModel> Parse(string filePath);

    }
}
