using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTagValidator.Interfaces
{
    public interface IBinaryParser
    {
        public List<string> Parse(string filePath);

    }
}
