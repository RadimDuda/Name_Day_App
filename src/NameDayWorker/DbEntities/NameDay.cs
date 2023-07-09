using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NameDayWorker.DbEntities
{
    public class NameDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Name> Names { get; set; }
    }
}