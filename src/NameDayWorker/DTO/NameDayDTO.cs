using System.Collections.Generic;
using System;

namespace playground_first2.DTO
{
    public class NameDayDTO
    {
        public DateTime Date { get; set; }

        public List<NameDTO> Names { get; set; }
    }
}
