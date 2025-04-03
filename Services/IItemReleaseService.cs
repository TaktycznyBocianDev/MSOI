using MSOI.Models;

namespace MSOI.Services
{
    public interface IItemReleaseService
    {

        public Task<List<ItemReleaseModel>> GetItemReleases(int? id = null, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null);
        public Task<bool> InsertReleaseItem(ItemReleaseModel itemType);
        public Task<bool> UpdateItemRelease(int id, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null);
        public Task<bool> DeleteData(int id);

    }
}
