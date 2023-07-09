using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NameDayWorker.DbEntities;

namespace NameDayWorker.DbFunctions
{
    class NameDayLoader
    {
        public List<NameDay> LoadNameDays(StreamReader streamReader)
        {
            var nameDays = new List<NameDay>();

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                Match match = Regex.Match(line, @"^(\d+\.\s*\d+\.\s*)(.+)$");

                if (match.Success)
                {
                    var datePart = match.Groups[1].Value.Trim();
                    var namesPart = match.Groups[2].Value.Trim();

                    datePart = Regex.Replace(datePart, @"\s+", "");

                    const int leapYearExample = 2020;

                    if (DateTime.TryParseExact(datePart + leapYearExample, "d.M.yyyy", null, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        var nameDay = new NameDay()
                        {
                            Date = parsedDate,
                            Names = namesPart.Split(',').Select(name => new Name { Value = name.Trim() }).ToList()
                        };
                        nameDays.Add(nameDay);
                    }
                }
            }

            return nameDays;
        }
    }
}
