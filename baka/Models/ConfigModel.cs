using System;
using System.IO;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace baka.Models
{
    public class ConfigModel
    {
        public static ConfigModel GetConfig(string ConfigFileName)
        {
            return JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(ConfigFileName));
        }
    }
}