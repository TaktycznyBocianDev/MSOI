using Dapper;
using DataLibrary;
using MSOI.Models;
using MySql.Data.MySqlClient;
using System.Text;

namespace MSOI.Repositories.RepositoryImplementations
{
    public class ItemTypeRepository : IItemTypeRepository
    {

        private readonly IDataAccess _data;
        private readonly string _connection;

        public ItemTypeRepository(IDataAccess data, IConfiguration connection)
        {
            _data = data;
            _connection = connection.GetConnectionString("default");
        }


        public async Task<List<ItemTypeModel>> GetItemTypes(int? id = null, string? item_name = null, int? default_replacement_period = null, string? item_description = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("SELECT * FROM item_type WHERE 1=1");

            if (id != null) { sql.Append(" AND id LIKE @id"); parameters.Add("id", $"{id}"); }
            if (!string.IsNullOrEmpty(item_name)) { sql.Append(" AND item_name LIKE @item_name"); parameters.Add("item_name", $"%{item_name}%"); }
            if (default_replacement_period != null) { sql.Append(" AND default_replacement_period LIKE @default_replacement_period"); parameters.Add("default_replacement_period", $"{default_replacement_period}"); }
            if (!string.IsNullOrEmpty(item_description)) { sql.Append(" item_description LIKE @item_description"); parameters.Add("item_description", $"%{item_description}%"); }

            return await _data.LoadData<ItemTypeModel, dynamic>(sql.ToString(), parameters, _connection);
        }


        public async Task<bool> InsertData(ItemTypeModel itemType)
        {
            string sql = "INSERT INTO item_type (item_name, default_replacement_period, item_description) VALUES (@item_name, @default_replacement_period, @item_description);";

            int rowsInserted = await _data.SaveData(sql, itemType, _connection);
            return rowsInserted > 0;
        }

        public async Task<bool> UpdateData(int id, string? item_name = null, int? default_replacement_period = null, string? item_description = null)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder("UPDATE item_type SET");

            if (!string.IsNullOrWhiteSpace(item_name)) { sql.Append(" item_name = @item_name,"); parameters.Add("item_name", item_name); }
            if (default_replacement_period != null) { sql.Append(" default_replacement_period = @default_replacement_period,"); parameters.Add("default_replacement_period", default_replacement_period.Value); }
            if (!string.IsNullOrWhiteSpace(item_description)) { sql.Append(" item_description = @item_description,"); parameters.Add("item_description", item_description); }

            sql.Length--;
            sql.Append(" WHERE id = @Id;");
            parameters.Add("Id", id);

            int rowsUpdated = await _data.SaveData(sql.ToString(), parameters, _connection);
            return rowsUpdated > 0;
        }


        public async Task<bool> DeleteData(ItemTypeModel itemType)
        {
            string sql = "DELETE FROM item_type WHERE id = @Id";
            var parameters = new { itemType.Id };

            try
            {
                int rowsDeleted = await _data.SaveData(sql, parameters, _connection);
                return rowsDeleted > 0;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451) //Cannot delete or update a parent row"
                {
                    throw new InvalidOperationException("Nie można usunąć przedmiotów tego typu, ponieważ istnieją pracownicy wciaż go posiadający.");
                }
                throw;
            }
        }

    }
}
