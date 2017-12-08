using System;
using baka.Models;

namespace baka {
    public static class Globals {
        public const string ConfigFileName = "baka_config.json";
        public static ConfigModel Config { get; set; }
        public static void Initliaze () {
            Config = ConfigModel.GetConfig (ConfigFileName);
        }
    }
}