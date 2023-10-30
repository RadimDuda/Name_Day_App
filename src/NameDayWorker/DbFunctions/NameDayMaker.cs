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
            var date = nameDayDto.Date.Date;

            var nameDay = new NameDay
            {
                Date = date,
                Names = nameDayDto.Names.Select(n => new Name { Value = n.Value }).ToList()
            };

            dbContext.NameDays.Add(nameDay);
            dbContext.SaveChanges();

            return nameDay;
        }
    }
}
