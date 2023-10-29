using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NameDayWorker.DbEntities;

namespace NameDayWorker.DbFunctions {
    class ByDateFinder {
        private DateTime _searchDate;

        public bool IsInvalidDate { get; set; }
        public bool IsInvalidFormattedDate { get; set; }
        public bool IsNoNameDay { get; set; }

        private readonly NameDayDbContext _dbContext;

        public ByDateFinder(NameDayDbContext dbContext) {
            _dbContext = dbContext;
        }

        public NameDay FindByDate(string date) {
            const string pattern = @"^\d+\.\s*\d+\.$";

            if (Regex.IsMatch(date, pattern)) {
                var formattedInputDate = Regex.Replace(date, @"\s+", "");

                const int leapYearExample = 2020;

                if (DateTime.TryParseExact(formattedInputDate + leapYearExample, "d.M.yyyy", null, DateTimeStyles.None, out DateTime searchDate)) {
                    if (searchDate.Month == 2 && searchDate.Day == 29) {
                        IsNoNameDay = true;
                        return null;
                    }

                    _searchDate = searchDate;

                    var matchingNameDay = _dbContext.NameDays
                        .Include(i => i.Names)
                        .FirstOrDefault(nameDay => nameDay.Date.Date == searchDate.Date);

                    return matchingNameDay;
                }
                else {
                    IsInvalidFormattedDate = true;
                }
            }
            else {
                IsInvalidDate = true;
            }

            return null;
        }

        public List<string> GetNamesBySpecificDate(NameDay nameDay) {
            if (nameDay != null) {
                return nameDay.Names.Select(n => n.Value).ToList();
            }

            return new List<string>();
        }
    }
}
