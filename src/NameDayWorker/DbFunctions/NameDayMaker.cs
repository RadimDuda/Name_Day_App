using NameDayWorker.DbEntities;
using NameDayWorker.DbFunctions;
using System.Linq;
using playground_first2.DTO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NameDayWorker.Controllers
{
    public static class NameDayMaker
    {
        public static NameDay CreateNameDayFromDto(NameDayDTO nameDayDto, NameDayDbContext dbContext)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(nameDayDto, options);
            var deserializedDto = JsonSerializer.Deserialize<NameDayDTO>(json, options);

            var date = deserializedDto.Date.Date;

            var nameDay = new NameDay
            {
                Date = date,
                Names = deserializedDto.Names.Select(n => new Name { Value = n.Value }).ToList()
            };

            dbContext.NameDays.Add(nameDay);
            dbContext.SaveChanges();

            return nameDay;
        }
    }
}
