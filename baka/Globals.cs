using System;
using baka.Models;
using Amazon.S3;
using Amazon.S3.Model;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.Transfer;

namespace baka
{
    public static class Globals
    {
        public const string ConfigFileName = "baka_config.json";
        public static ConfigModel Config { get; set; }
        public static AmazonS3Config S3Config { get; set; }
        public static AmazonS3Client S3Client { get; set; }

        public static TransferUtility S3Utility { get; set; }

        public static void Initliaze()
        {
            Config = ConfigModel.GetConfig(ConfigFileName);

            S3Config = new AmazonS3Config();
            if (Config.S3ServiceURL != null)
                S3Config.ServiceURL = Config.S3ServiceURL;

            S3Client = new AmazonS3Client(Config.S3AccessKey, Config.S3SecretKey, S3Config);

            S3Utility = new TransferUtility(S3Client);

        }

        public static async Task<bool> UploadFile(Stream stream, string FileId, string ContentType)
        {
            try
            {
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                request.BucketName = Config.S3BucketName;
                request.Key = FileId;
                request.InputStream = stream;
                request.ContentType = ContentType;

                await S3Utility.UploadAsync(request);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Stream> GetFile(string FileId)
        {
            try
            {
                TransferUtilityOpenStreamRequest request = new TransferUtilityOpenStreamRequest();
                request.BucketName = Config.S3BucketName;
                request.Key = FileId;

                return await S3Utility.OpenStreamAsync(request);
            }
            catch
            {
                return null;
            }
        }


    }
}