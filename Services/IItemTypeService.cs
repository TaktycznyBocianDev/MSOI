using MSOI.Models;

namespace MSOI.Services
{
    public interface IItemTypeService
    {

        public Task<List<ItemTypeModel>> GetItemTypes(int? id = null, string? item_name = null, int? default_replacement_period = null, string? item_description = null);
        public Task<bool> InsertData(ItemTypeModel itemType);
        public Task<bool> UpdateData(int id, string? item_name = null, int? default_replacement_period = null, string? item_description = null);
        public Task<bool> DeleteData(ItemTypeModel itemType);
    }
}
