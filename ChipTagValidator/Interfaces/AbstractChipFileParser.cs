using ChipTagValidator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChipTagValidator.Interfaces
{
    public abstract class AbstractChipFileParser<T>
    {
        protected abstract T LoadFile(string path);
        public abstract List<TagModel> Parse(string filePath);

    }
}
