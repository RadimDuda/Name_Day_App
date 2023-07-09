namespace NameDayWorker.DbEntities
{
    public class Name
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int NameDayId { get; set; }
        public NameDay NameDay { get; set; }
    }
}

