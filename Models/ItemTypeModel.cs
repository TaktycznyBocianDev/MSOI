namespace MSOI.Models
{
    public class ItemTypeModel
    {

        public int Id { get; set; }
        public string Item_name { get; set; } = string.Empty;
        public int Default_replacement_period { get; set; }
        public string Item_description { get; set; } = string.Empty;

    }
}
