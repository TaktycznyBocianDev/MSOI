using MSOI.Models;

namespace MSOI.Repositories
{
    public interface IWorkerItemRepository
    {
        public Task<List<WorkerItemDTO>> GetItemsByWorker(int worker_id);
        public Task<List<WorkerItemDTO>> GetWorkersByItem(int item_id);

    }
}
