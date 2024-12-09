using ChipTagValidator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChipTagValidator
{
    public class Configuration
    {

        private static Configuration _config = null;

        public static Configuration Config
        {
            get
            {
                if (_config == null) {
                    _config = new Configuration();
                }
                return _config;
            }
        }
        public ConfigModel ConfigModel { get; private set; }

        private Configuration() {

        }

        public void LoadConfig(string Path) {

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string jsonString = File.ReadAllText("..\\config.json");


            ConfigModel = JsonSerializer.Deserialize<ConfigModel>(jsonString, options);
            
        }


    }
}
