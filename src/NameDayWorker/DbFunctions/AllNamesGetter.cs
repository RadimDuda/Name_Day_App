using Microsoft.EntityFrameworkCore;
using NameDayWorker.DbEntities;
using NameDayWorker.DbFunctions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace NameDayWorker.Controllers
{
    public class AllNamesGetter
    {
        private readonly NameDayDbContext _dbContext;

        public AllNamesGetter(NameDayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetAllNameDaysFromDatabase()
        {
            var nameDays = _dbContext.NameDays.Include(nd => nd.Names).ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            var json = JsonSerializer.Serialize(nameDays, options);

            return json;
        }
    }
}

