using System;
using System.IO;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace baka.Models
{
    public class ConfigModel
    {
        [J("database_name")]
        public string DbName { get; set; }

        [J("root_token")]
        public string RootToken { get; set; }

        [J("debug")]
        public bool IsDebug { get; set; }
        [J("log_interval")]
        public int LogInterval { get; set; }
        [J("log_path")]
        public string LogPath { get; set; }
        public static ConfigModel GetConfig(string ConfigFileName)
        {
            return JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(ConfigFileName));
        }
    }
}