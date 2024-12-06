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
    public class BinaryParser : IBinaryParser
    {

        private string _chipDataDelimiter;

        public BinaryParser(string chipDataDelimiter) {  _chipDataDelimiter = chipDataDelimiter; }

        public List<string> Parse(string filePath)

        {
            var stream = File.Open(filePath, FileMode.Open);
            try
            {

                Log.Information($"Trying to parse the binary file on {filePath}");
                int cardCounterForLog = 1;
                //StreamReader streamReader = new StreamReader(filePath);
                BinaryReader streamReader = new BinaryReader(stream);
                StringBuilder currentstring = new StringBuilder();
                StringBuilder chipDataString = new StringBuilder();
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
                    Log.Debug($" cardCounterForLog {cardCounterForLog} chipDataString, {chipDataString.ToString()}, currentInt {currentInt}, data {data}, chipDataStart {chipDataStart}");
                    Log.Debug("chipDatastrings[]");
                    foreach (string s in chipDatastrings) {
                        Log.Debug($"{s}");
                    }

                    if (currentstring.Length >= _chipDataDelimiter.Length)
                    {
                        string curr = currentstring.ToString().Substring(currentstring.Length - _chipDataDelimiter.Length);
                        if (currentstring.ToString().Substring(currentstring.Length - _chipDataDelimiter.Length) == _chipDataDelimiter)
                            chipDataStart = true;
                    }
                    currentstring.Append((char)currentInt);


                    if (chipDataStart)
                    {
                        chipDataString.Append(currentInt.ToString("X2"));
                        //char data = (char)currentCharacter;
                        if ((char)currentInt == '#' && chipDataString.Length > 10)
                        {
                            string lastCharactersOfBuilder = HexToChar(chipDataString.ToString().Substring(chipDataString.Length - 10));

                            if (lastCharactersOfBuilder.Equals(cardEnd))
                            {
                                Log.Information($"Reached end of card {cardCounterForLog++}");
                                chipDataStart = false;
                                chipDatastrings.Add(chipDataString.ToString());
                                chipDataString.Clear();

                            }
                        }

                    }


                    
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
