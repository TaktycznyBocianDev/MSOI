using MSOI.Models;

namespace MSOI.Repositories
{
    /// <summary>
    /// CRUD operations for workers, operation to check for existing PESEL
    /// </summary>
    public interface IWorkerRepository
    {
        
        public Task<List<WorkerModel>> GetWorkers(int? id = null, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null);
        public Task<bool> DoPeselExist(string pesel);
        public Task<bool> InsertData(WorkerModel worker);
        public Task<bool> UpdateData(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null);
        public Task<bool> DeleteData(int id);

    }
}
