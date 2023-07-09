using System;
using System.Collections.Generic;
using NameDayWorker.DbEntities;

namespace NameDayWorker.DbFunctions
{
    public class DbImporter
    {
        private readonly NameDayDbContext _dbContext;

        public DbImporter(NameDayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ImportDataToDatabase(List<NameDay> nameDays)
        {
            _dbContext.Database.EnsureCreated();

            foreach (var nameDay in nameDays)
            {
                _dbContext.NameDays.Add(nameDay);
            }

            _dbContext.SaveChanges();

            Console.WriteLine("Data byla úspěšně importována do databáze.");
        }
    }


}
