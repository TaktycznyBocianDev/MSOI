namespace MSOI.Models
{
    public class ItemReleaseModel
    {
        public int Id { get; set; }
        public int Worker_id { get; set; }
        public int Item_type_id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public DateTime Release_date { get; set; }
        public DateTime Exchange_date { get; set; }

    }
}
