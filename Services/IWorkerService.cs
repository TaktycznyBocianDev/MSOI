using MSOI.Models;

namespace MSOI.Services
{
    public interface IWorkerService
    {

        public Task<List<WorkerModel>> GetWorkers(int? id = null, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null);
        public Task<bool> InsertWorker(WorkerModel worker);
        public Task<bool> UpdateWorker(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null);
        public Task<bool> DeleteWorker(int id);
    }
}
