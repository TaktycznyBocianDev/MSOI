using Dapper;
using DataLibrary;
using MSOI.Components.Pages;
using MSOI.Models;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MSOI.Services
{
    public class WorkerService
    {

        private readonly IDataAccess _data;
        private readonly string _connection;

        public WorkerService(IDataAccess data, IConfiguration connection)
        {
            _data = data;
            _connection = connection.GetConnectionString("default");
        }

        public async Task<List<WorkerModel>> GetWorkers(string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("SELECT * FROM workers WHERE 1=1"); 

            if (!string.IsNullOrWhiteSpace(name)) { sql.Append(" AND worker_name LIKE @name"); parameters.Add("name", $"%{name}%"); }
            if (!string.IsNullOrWhiteSpace(surname)) { sql.Append(" AND worker_surname LIKE @surname"); parameters.Add("surname", $"%{surname}%"); }
            if (!string.IsNullOrWhiteSpace(position)) { sql.Append(" AND position LIKE @position"); parameters.Add("position", $"%{position}%"); }
            if (employment_date.HasValue) { sql.Append(" AND employment_date = @employment_date"); parameters.Add("employment_date", $"{employment_date.Value}"); }
            if (!string.IsNullOrWhiteSpace(pesel)) { sql.Append(" AND pesel = @pesel"); parameters.Add("pesel", $"{pesel}"); } 
            

            return await _data.LoadData<WorkerModel, dynamic>(sql.ToString(), parameters, _connection);
        }

        public async Task InsertData(WorkerModel worker)
        {
            string sql = "INSERT INTO workers (worker_name, worker_surname, position, employment_date, pesel) VALUES (@worker_name, @worker_surname, @position, @employment_date, @pesel);";
            await _data.SaveData(sql, worker, _connection);
        }

        public async Task UpdateData(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("UPDATE workers SET");

            if (!string.IsNullOrWhiteSpace(name)) { sql.Append(" worker_name = @Worker_name,"); parameters.Add("Worker_name", name); }
            if (!string.IsNullOrWhiteSpace(surname)) { sql.Append(" worker_surname = @Worker_surname,"); parameters.Add("Worker_surname", surname); }
            if (!string.IsNullOrWhiteSpace(position)) { sql.Append(" position = @Position,"); parameters.Add("Position", position); }
            if (employment_date.HasValue) { sql.Append(" employment_date = @Employment_date,"); parameters.Add("Employment_date", employment_date.Value); }
            if (!string.IsNullOrWhiteSpace(pesel)) { sql.Append(" pesel = @Pesel,"); parameters.Add("Pesel", pesel); }

            sql.Length--; 
            sql.Append(" WHERE id = @Id;");
            parameters.Add("Id", id);

            await _data.SaveData(sql.ToString(), parameters, _connection);
        }

        public async Task DeleteData(WorkerModel workerModel)
        {
            string sql = "DELETE FROM workers WHERE id = @Id";
            await _data.SaveData(sql, workerModel, _connection);
        }

    }
}
