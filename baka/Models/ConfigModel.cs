using System;
using System.IO;
using Amazon;
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
        [J("s3_access_key")]
        public string S3AccessKey { get; set; }
        [J("s3_secret_key")]
        public string S3SecretKey { get; set; }
        [J("s3_service_url")]
        public string S3ServiceURL { get; set; }
        [J("s3_bucket_name")]
        public string S3BucketName { get; set; }

        public static ConfigModel GetConfig(string ConfigFileName)
        {
            return JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(ConfigFileName));
        }
    }
}