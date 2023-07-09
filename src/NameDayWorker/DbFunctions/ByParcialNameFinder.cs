using Microsoft.EntityFrameworkCore;
using NameDayWorker.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NameDayWorker.DbFunctions
{
    class ByParcialNameFinder
    {
        public List<NameDay> FindByPartialName(string pattern, NameDayDbContext dbContext)
        {
            var lowercasePattern = pattern.ToLower();

            var matchingNameDays = dbContext.NameDays
                .Include(nd => nd.Names)  
                .Where(nameDay => nameDay.Names
                    .Any(name => EF.Functions.Like(
                        name.Value.ToLower(),
                        lowercasePattern.Replace('*', '%')) || name.Value.ToLower().Contains(lowercasePattern)))
                .OrderBy(nameDay => nameDay.Date)
                .Select(nameDay => new NameDay
                {
                    Id = nameDay.Id,
                    Date = nameDay.Date,
                    Names = nameDay.Names
                        .Where(n => n.Value != null && (EF.Functions.Like(
                            n.Value.ToLower(),
                            lowercasePattern.Replace('*', '%')) || n.Value.ToLower().Contains(lowercasePattern)))
                        .Select(n => new Name { Value = n.Value })
                        .ToList()
                })
                .Where(nameDay => nameDay.Names.Count > 0)
                .ToList();

            return matchingNameDays;
        }
    }
}
