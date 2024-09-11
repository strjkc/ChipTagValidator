using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsParser.Classes
{
    internal class BinaryParser : IParser
    {

        public List<String> Parse(string filePath)
        {
            return ParseFile(filePath);
        }


        public List<string> ParseFile(string filePath)

        {
            var stream = File.Open(filePath, FileMode.Open);
            try
            {


                //StreamReader streamReader = new StreamReader(filePath);
                BinaryReader streamReader = new BinaryReader(stream);
                StringBuilder currentstring = new StringBuilder();
                string cardEnd = "#END#";
                Boolean chipDataStart = false;
                List<string> chipDatastrings = new List<string>();
                //int currentCharacter = streamReader.Read();
                byte bytes = streamReader.ReadByte();
                while (streamReader.BaseStream.Position != streamReader.BaseStream.Length)
                {
                    int currentInt = (Int16)bytes;

                    char data = (char)currentInt;

                    if (chipDataStart)
                    {
                        currentstring.Append(currentInt.ToString("X2"));
                        //char data = (char)currentCharacter;
                        if ((char)currentInt == '#' && currentstring.Length > 10)
                        {
                            string lastCharactersOfBuilder = HexToChar(currentstring.ToString().Substring(currentstring.Length - 10));

                            if (lastCharactersOfBuilder.Equals(cardEnd))
                            {
                                chipDataStart = false;
                                chipDatastrings.Add(currentstring.ToString());
                                currentstring.Clear();

                            }
                        }

                    }

                    if ((char)currentInt == '{')
                        chipDataStart = true;
                    bytes = streamReader.ReadByte();
                }
                return chipDatastrings;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
                return null;
            }
        }


        private string HexToChar(string hexString)
        {
            StringBuilder sb = new();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string sbstr = hexString.Substring(i, 2);
                int x = Int32.Parse(sbstr, System.Globalization.NumberStyles.HexNumber);
                char c = (char)x;
                sb.Append(c);


            }
            return sb.ToString();
        }





    }
}
