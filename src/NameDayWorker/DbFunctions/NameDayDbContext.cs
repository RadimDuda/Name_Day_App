using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using NameDayWorker.DbEntities;
using Microsoft.Extensions.Configuration;
using System;

namespace NameDayWorker.DbFunctions
{
    public class NameDayDbContext : DbContext
    {
        public DbSet<NameDay> NameDays { get; set; }
        public DbSet<Name> Names { get; set; }

        private readonly IConfiguration _configuration;
        public NameDayDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public NameDayDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var connectionString = _configuration.GetConnectionString(environment == "Production" ? "NameDayDbConnectionProduction" : "NameDayDbConnection");

            if (environment == "Production") {
                optionsBuilder.UseSqlServer(connectionString);
            }
            else {
                optionsBuilder.UseSqlite(connectionString);
            }
        }
    }
}