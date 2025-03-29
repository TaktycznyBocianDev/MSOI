using Dapper;
using DataLibrary;
using MSOI.Models;
using MySql.Data.MySqlClient;
using System.Text;

namespace MSOI.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly IDataAccess _data;
        private readonly string _connection;

        public WorkerRepository(IDataAccess data, IConfiguration connection)
        {
            _data = data;
            _connection = connection.GetConnectionString("default");
        }

        public async Task<List<WorkerModel>> GetWorkers(int? id = null, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("SELECT * FROM workers WHERE 1=1");

            if (id != null) { sql.Append(" AND id = @id"); parameters.Add("id", $"{id}"); }
            if (!string.IsNullOrWhiteSpace(name)) { sql.Append(" AND worker_name LIKE @name"); parameters.Add("name", $"%{name}%"); }
            if (!string.IsNullOrWhiteSpace(surname)) { sql.Append(" AND worker_surname LIKE @surname"); parameters.Add("surname", $"%{surname}%"); }
            if (!string.IsNullOrWhiteSpace(position)) { sql.Append(" AND position LIKE @position"); parameters.Add("position", $"%{position}%"); }
            if (employment_date.HasValue) { sql.Append(" AND employment_date = @employment_date"); parameters.Add("employment_date", $"{employment_date.Value}"); }
            if (!string.IsNullOrWhiteSpace(pesel)) { sql.Append(" AND pesel = @pesel"); parameters.Add("pesel", $"{pesel}"); }


            return await _data.LoadData<WorkerModel, dynamic>(sql.ToString(), parameters, _connection);
        }

        public async Task<bool> InsertData(WorkerModel worker)
        {
            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string sql = "INSERT INTO workers (worker_name, worker_surname, position, employment_date, pesel) VALUES (@worker_name, @worker_surname, @position, @employment_date, @pesel);";

                        int rowsInserted = await _data.SaveData(sql, worker, _connection, transaction);

                        await transaction.CommitAsync();
                        return rowsInserted > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }


        public async Task<bool> UpdateData(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        var updates = new List<string>();

                        if (!string.IsNullOrWhiteSpace(name)) { updates.Add("worker_name = @Worker_name"); parameters.Add("Worker_name", name); }
                        if (!string.IsNullOrWhiteSpace(surname)) { updates.Add("worker_surname = @Worker_surname"); parameters.Add("Worker_surname", surname); }
                        if (!string.IsNullOrWhiteSpace(position)) { updates.Add("position = @Position"); parameters.Add("Position", position); }
                        if (employment_date.HasValue) { updates.Add("employment_date = @Employment_date"); parameters.Add("Employment_date", employment_date.Value); }
                        if (!string.IsNullOrWhiteSpace(pesel)) { updates.Add("pesel = @Pesel"); parameters.Add("Pesel", pesel); }

                        if (!updates.Any()) return false; // no changes at all

                        string sql = $"UPDATE workers SET {string.Join(", ", updates)} WHERE id = @Id;";
                        parameters.Add("Id", id);

                        int rowsUpdated = await _data.SaveData(sql.ToString(), parameters, _connection, transaction);
                        return rowsUpdated > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

            }

        }

        public async Task<bool> DeleteData(int id)
        {

            using (var connection = new MySqlConnection(_connection))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    string sql = "DELETE FROM workers WHERE id = @Id";

                    try
                    {
                        int rowsDeleted = await _data.SaveData(sql, new { Id = id }, _connection, transaction);
                        return rowsDeleted > 0;
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1451) //Cannot delete or update a parent row"
                        {
                            throw new InvalidOperationException("Nie można usunąć pracownika, ponieważ posiada nieoddane przedmioty.");
                        }
                        await transaction.RollbackAsync();
                        throw;
                    }

                }


            }
        }

        /// <summary>
        /// Returns true if PESEL provided exist for any other worker. 
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public async Task<bool> DoPeselExist(string pesel)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM workers WHERE pesel = @Pesel);";
            return await _data.ExecuteScalarAsync<bool, dynamic>(sql, new { Pesel = pesel }, _connection);
        }


    }
}
