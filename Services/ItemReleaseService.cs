using Dapper;
using DataLibrary;
using MSOI.Models;
using MySql.Data.MySqlClient;
using System.Text;

namespace MSOI.Services
{
    public class ItemReleaseService
    {

        private readonly IDataAccess _data;
        private readonly string _connection;

        public ItemReleaseService(IDataAccess data, IConfiguration connection)
        {
            _data = data;
            _connection = connection.GetConnectionString("default");
        }

        public async Task<List<ItemReleaseModel>> GetItemReleases(int? id = null, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("SELECT * FROM item_release WHERE 1=1");

            if (id != null) { sql.Append(" AND id = @id"); parameters.Add("id", id); }
            if (worker_id != null) { sql.Append(" AND worker_id = @worker_id"); parameters.Add("worker_id", worker_id); }
            if (item_type_id != null) { sql.Append(" AND item_type_id = @item_type_id"); parameters.Add("item_type_id", item_type_id); }
            if (!string.IsNullOrEmpty(size)) { sql.Append(" AND size LIKE @size"); parameters.Add("size", $"%{size}%"); }
            if (!string.IsNullOrEmpty(color)) { sql.Append(" AND color LIKE @color"); parameters.Add("color", $"%{color}%"); }
            if (release_date.HasValue) { sql.Append(" AND release_date = @release_date"); parameters.Add("release_date", release_date.Value.Date); }
            if (exchange_date.HasValue) { sql.Append(" AND exchange_date = @exchange_date"); parameters.Add("exchange_date", exchange_date.Value.Date); }

            return await _data.LoadData<ItemReleaseModel, dynamic>(sql.ToString(), parameters, _connection);
        }


        public async Task<bool> InsertReleaseItem(ItemReleaseModel itemType)
        {
            string sql = "INSERT INTO item_release (worker_id, item_type_id, size, color, release_date, exchange_date) VALUES (@worker_id, @item_type_id, @size, @color, @release_date, @exchange_date);";

            int rowsInserted = await _data.SaveData(sql, itemType, _connection);
            return rowsInserted > 0;
        }

        public async Task<bool> UpdateItemRelease(int id, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("UPDATE item_release SET");

            if (worker_id != null) { sql.Append(" worker_id = @worker_id,"); parameters.Add("worker_id", worker_id); }
            if (item_type_id != null) { sql.Append(" item_type_id = @item_type_id,"); parameters.Add("item_type_id", item_type_id); }
            if (!string.IsNullOrEmpty(size)) { sql.Append(" size = @size,"); parameters.Add("size", size); }
            if (!string.IsNullOrEmpty(color)) { sql.Append(" color = @color,"); parameters.Add("color", color); }
            if (release_date.HasValue) { sql.Append(" release_date = @release_date,"); parameters.Add("release_date", release_date.Value.Date); }
            if (exchange_date.HasValue) { sql.Append(" exchange_date = @exchange_date,"); parameters.Add("exchange_date", exchange_date.Value.Date); }

            sql.Length--;
            sql.Append(" WHERE id = @Id;");
            parameters.Add("Id", id);

            int rowsUpdated = await _data.SaveData(sql.ToString(), parameters, _connection);
            return rowsUpdated > 0;
        }

        public async Task<bool> DeleteData(ItemReleaseModel itemRelease)
        {
            string sql = "DELETE FROM item_release WHERE id = @Id";

            try
            {
                int rowsDeleted = await _data.SaveData(sql, itemRelease, _connection);
                return rowsDeleted > 0;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) // <- tutaj nie może to wystąpić, ale na razie niech zostanie
                {
                    throw new InvalidOperationException("Nie można usunąć tego powiązania!");
                }
                throw;
            }
        }
          
    }
}
