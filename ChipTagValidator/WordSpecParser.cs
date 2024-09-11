using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsParser.Classes;
using Microsoft.Office.Interop.Word;

namespace ChipTagValidator
{
    public class WordSpecParser
    {
        public List<TagModel> Parse(string file) {
            Application word = new Application();  
            Document doc = word.Documents.Open(file);
            doc.
        }
    }
}
