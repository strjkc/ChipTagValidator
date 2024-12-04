using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTagValidator.Interfaces;
using Serilog;
using Serilog.Core;

namespace TagsParser.Classes
{
    public class BinaryParser : IParser
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

                Log.Information($"Trying to parse the binary file on {filePath}");
                int cardCounterForLog = 1;
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
                    Log.Debug("State of variables");
                    Log.Debug($" cardCounterForLog {cardCounterForLog} currentstring, {currentstring.ToString()}, currentInt {currentInt}, data {data}, chipDataStart {chipDataStart}");
                    Log.Debug("chipDatastrings[]");
                    foreach (string s in chipDatastrings) {
                        Log.Debug($"{s}");
                    }


                    if (chipDataStart)
                    {
                        currentstring.Append(currentInt.ToString("X2"));
                        //char data = (char)currentCharacter;
                        if ((char)currentInt == '#' && currentstring.Length > 10)
                        {
                            string lastCharactersOfBuilder = HexToChar(currentstring.ToString().Substring(currentstring.Length - 10));

                            if (lastCharactersOfBuilder.Equals(cardEnd))
                            {
                                Log.Information($"Reached end of card {cardCounterForLog++}");
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
                Log.Information($"Exporting {chipDatastrings.Count} chip data strings:");
                foreach (string s in chipDatastrings)
                {
                    Log.Information($"{s}");

                }
                stream.Close();
                return chipDatastrings;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to parse binary file");
                Log.Error(ex.StackTrace);
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
