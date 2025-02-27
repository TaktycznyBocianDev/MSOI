using Dapper;
using DataLibrary;
using MSOI.Models;
using System.Text;

namespace MSOI.Services
{
    public class WorkerItemService
    {
        private readonly IDataAccess _data;
        private readonly string _connection;

        public WorkerItemService(IDataAccess data, IConfiguration connection)
        {
            _data = data;
            _connection = connection.GetConnectionString("default");
        }

        public async Task<List<WorkerItemDTO>> GetItemsByWorker(int worker_id)
        {
            var sql = @"
                SELECT ir.id, ir.worker_id, w.worker_name, w.worker_surname, w.position, w.employment_date, w.pesel,
                       ir.item_type_id, it.item_name, it.default_replacement_period, it.item_description,
                       ir.size, ir.color, ir.release_date, ir.exchange_date
                FROM item_release ir
                LEFT JOIN workers w ON ir.worker_id = w.id
                LEFT JOIN item_type it ON ir.item_type_id = it.id
                WHERE ir.worker_id = @worker_id;
            ";

            return await _data.LoadData<WorkerItemDTO, dynamic>(sql, new { worker_id }, _connection);
        }

        public async Task<List<WorkerItemDTO>> GetWorkersByItem(int item_id)
        {
            var sql = @"
                SELECT ir.id, ir.worker_id, w.worker_name, w.worker_surname, w.position, w.employment_date, w.pesel,
                       ir.item_type_id, it.item_name, it.default_replacement_period, it.item_description,
                       ir.size, ir.color, ir.release_date, ir.exchange_date
                FROM item_release ir
                LEFT JOIN workers w ON ir.worker_id = w.id
                LEFT JOIN item_type it ON ir.item_type_id = it.id
                WHERE it.id = @item_id;
            ";

            return await _data.LoadData<WorkerItemDTO, dynamic>(sql, new { item_id }, _connection);
        }
    }
}
