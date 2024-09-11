using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    internal interface IParser
    {
        public List<string> Parse(string filePath);

    }
}
