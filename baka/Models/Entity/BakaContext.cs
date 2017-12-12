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
            Permissions = new HashSet<PERMISSION>();
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public int Id { get; set; }

        public ICollection<BakaFile> Files { get; set; }

        public ICollection<BakaLink> Links { get; set; }

        public ICollection<PERMISSION> Permissions { get; set; }

        public DateTime Timestamp { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }

        public string InitialIp { get; set; }

        public double UploadLimitMB { get; set; }

        public bool Deleted { get; set; }

        public bool Disabled { get; set; }
    }

    public enum PERMISSION
    {   
        SU_UPLOAD_OBJECTS = 0,
        SU_UPLOAD_LINKS = 1,
        SU_RESET_TOKEN = 2,
        SU_VIEW_PRIVATE_ACCOUNT_INFO = 3,
        SU_CREATE_ACCOUNTS = 4,
        SU_DISABLE_ACCOUNTS = 5,
        SU_DELETE_ACCOUNTS = 6,
        SU_DELETE_OBJECTS = 7,
        SU_DELETE_LINKS = 8,
        SU_DELETE_SELF_OBJECTS = 9,
        SU_DELETE_SELF_LINKS = 10
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