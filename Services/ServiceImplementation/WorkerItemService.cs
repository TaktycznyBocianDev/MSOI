using Dapper;
using DataLibrary;
using MSOI.Models;
using MSOI.Repositories;
using System.Text;

namespace MSOI.Services.ServiceImplementation
{
    public class WorkerItemService : IWorkerItemService
    {

        private readonly IWorkerItemRepository _repository;
        public WorkerItemService(IWorkerItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WorkerItemDTO>> GetItemsByWorker(int worker_id)
        {
            
            List<WorkerItemDTO> workerItemDTOs = await _repository.GetItemsByWorker(worker_id);

            if (workerItemDTOs == null || workerItemDTOs.Count == 0)
            {
                throw new Exception("Wystąpił błąd podczas pobierania przedmiotów i przyporządkowanych im pracowników.");
            }

            return workerItemDTOs;

        }

        public async Task<List<WorkerItemDTO>> GetWorkersByItem(int item_id)
        {
            List<WorkerItemDTO> workerItemDTOs = await _repository.GetWorkersByItem(item_id);

            if (workerItemDTOs == null || workerItemDTOs.Count == 0)
            {
                throw new Exception("Wystąpił błąd podczas pobierania praciwników wg danego przedmiotu.");
            }

            return workerItemDTOs;
        }
    }
}
