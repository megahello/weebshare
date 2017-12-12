using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace baka.Models.Entity
{
    public class BakaContext : DbContext
    {
        public DbSet<BakaUser> Users { get; set; }

        public DbSet<BakaFile> Files { get; set; }

        public DbSet<BakaLink> Links { get; set; }

        public DbSet<BakaPermission> Permissions { get; set; }

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
            Links = new HashSet<BakaLink>();
            Permissions = new HashSet<BakaPermission>();
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public int Id { get; set; }

        public ICollection<BakaFile> Files { get; set; }

        public ICollection<BakaLink> Links { get; set; }

        public ICollection<BakaPermission> Permissions { get; set; }

        public DateTime Timestamp { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public string InitialIp { get; set; }

        public double UploadLimitMB { get; set; }

        public bool Deleted { get; set; }

        public bool Disabled { get; set; }

        [NotMapped]
        public ICollection<string> PermissionsList
        {
            get
            {
                var list = new HashSet<string>();
                foreach (BakaPermission perm in Permissions)
                {
                    list.Add(perm.Data);
                }

                return list;
            }
        }
    }

    public class BakaPermission
    {   
        [JsonIgnore]
        public BakaUser User { get; set; }
        
        [Key]
        public string Data { get; set; }
    }

    public class BakaFile
    {
        public string BackendFileId { get; set; }

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Filename { get; set; }

        public string Extension { get; set; }

        public string IpUploadedFrom { get; set; }

        public bool Deleted { get; set; }

        public BakaUser Uploader { get; set; }

        public DateTime Timestamp { get; set; }

        public double FileSizeMB { get; set; }

        [NotMapped]
        public string ContentType
        {
            get
            {
                return BakaMime.GetMimeType(Extension);
            }
        }
    }

    public class BakaLink
    {
        public string Destination { get; set; }

        public string UploadedFromIp { get; set; }

        public bool Deleted { get; set; }

        public string ExternalId { get; set; }

        public int Id { get; set; }

        public BakaUser Uploader { get; set; }

        public DateTime Timestamp { get; set; }
    }
}