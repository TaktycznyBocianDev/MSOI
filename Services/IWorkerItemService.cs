using MSOI.Models;

namespace MSOI.Services
{
    public interface IWorkerItemService
    {
        public Task<List<WorkerItemDTO>> GetItemsByWorker(int worker_id);
        public Task<List<WorkerItemDTO>> GetWorkersByItem(int item_id);

    }
}
