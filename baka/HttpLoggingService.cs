using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace baka
{
    public class HttpLoggingService
    {
        //This class is a rly bad class I know, but it's custom and yeah.
        public HttpLoggingService()
        {

        }

        [J("http_request_log")]
        private List<BakaRequest> Requests = new List<BakaRequest>();

        [JsonIgnore]
        private int request_count { get; set; }

        public async Task Log(BakaRequest request)
        {
            Requests.Add(request);

            request_count++;

            await Task.Run(() =>
            {
                if (Globals.Config.LogRequestsToConsole)
                    Console.WriteLine(JsonConvert.SerializeObject(request));
                CheckAndSaveLogs();
            });
        }

        private async void CheckAndSaveLogs()
        {
            if (request_count > Globals.Config.LogInterval)
            {
                request_count = default(int);
                string json_data = JsonConvert.SerializeObject(this);

                using (var writer = new StreamWriter(File.Create(Path.Combine(Globals.Config.LogPath, $"log_{DateTime.Now.ToFileTimeUtc()}.json"))))
                {
                    await writer.WriteLineAsync(json_data);
                }

                Requests.Clear();
            }
        }
    }

    public class BakaRequest
    {
        [J("url")]
        public string DisplayUrl { get; set; }

        [J("auth_header")]
        public string AuthHeader { get; set; }

        [J("remote_ip")]
        public string RemoteIp { get; set; }

        [J("method")]
        public string Method { get; set; }
        
        [J("timestamp")]
        public string Timestamp { get; set; }
    }
}