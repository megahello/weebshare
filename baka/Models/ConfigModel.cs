using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using baka.Models.Entity;
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

        [J("log_requests_to_console")]
        public bool LogRequestsToConsole { get; set; }

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

        [J("s3_key_prefix")]
        public string S3KeyPrefix { get; set; }

        [J("s3_set_public_read")]
        public bool SetS3Public { get; set; }

        [J("preserve_deleted_files")]
        public bool PreserveDeletedFiles { get; set; }

        [J("give_default_permissions")]
        public bool GiveDefaultPermissions { get; set; }

        [J("default_permissions")]
        public IEnumerable<PERMISSION> DefaultPermissions { get; set; }

        [J("jwt_secret_key")]
        public string JWTKey { get; set; }

        [J("id_length")]
        public int IdLength { get; set; }

        [J("default_root_permissions")]
        public IEnumerable<PERMISSION> DefaultRootPermissions { get; internal set; }

        public static ConfigModel GetConfig(string ConfigFileName)
        {
            return JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(ConfigFileName));
        }
    }
}