using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace baka.Models.Entity
{
    public class BakaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            OptionsBuilder.UseSqlite($"Data Source = {Globals.Config.DbName}");
        }
    }

    public class BakaUser
    {
        public BakaUser()
        {
            Files = new HashSet<BakaFile>();
        }
        public string Name { get; set; }
        public string Username { get; set; }
        public int Id { get; set; }
        public ICollection<BakaFile> Files { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }
        public string IntialIp { get; set; }
        public double UploadLimitMB { get; set; }

    }

    public class BakaFile
    {
        public string BackendFileId { get; set; }
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Filename { get; set; }
        public string IpUploadedFrom { get; set; }
        public bool Deleted { get; set; }
        public BakaUser Uploader { get; set; }
        public double FileSizeMB { get; set; }
    }
}