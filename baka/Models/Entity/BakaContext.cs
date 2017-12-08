using System;
using Microsoft.EntityFrameworkCore;

namespace baka.Models.Entity
{
    public class BakaContext : DbContext
    {
        public override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            OptionsBuilder.UseSqlite(Globals.Config.DbName);
        } 
    }
}