namespace MSOI.Models
{
    public class WorkerItemDTO
    {
        //Id from the Item Release table
        public int Id { get; set; }

        //Data of the worker
        public int Worker_id { get; set; }
        public string Worker_name { get; set; } = string.Empty;
        public string Worker_surname { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime Employment_date { get; set; }
        public string Pesel { get; set; } = string.Empty;

        //Data of the item
        public int Item_type_id { get; set; }
        public string Item_name { get; set; } = string.Empty;
        public int Default_replacement_period { get; set; }
        public string Item_description { get; set; } = string.Empty;

        public string Size { get; set; }
        public string Color { get; set; }

        //Time stamps
        public DateTime Release_date { get; set; }
        public DateTime Exchange_date { get; set; }

    }
}
