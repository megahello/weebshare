using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace baka.Models
{
    public class NewUserModel
    {
        [J("username")]
        public string Username { get; set; }

        [J("full_name")]
        public string Name { get; set; }

        [J("upload_limit")]
        public double UploadLimit { get; set; }
    }
}