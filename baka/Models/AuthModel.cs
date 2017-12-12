using System;
using baka.Models.Entity;

namespace baka.Models
{
    public class AuthModel
    {
        public BakaUser User { get; set; }
        
        public bool Authorized { get; set; }

        public string Reason { get; set; }
    }
}