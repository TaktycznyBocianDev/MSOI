namespace MSOI.Models
{
    public class WorkerModel
    {

        public int Id { get; set; }
        public string Worker_name { get; set; } = string.Empty;
        public string Worker_surname { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime Employment_date { get; set; }
        public string Pesel { get; set; } = string.Empty;


    }
}
