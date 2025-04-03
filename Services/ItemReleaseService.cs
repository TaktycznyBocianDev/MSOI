using Dapper;
using DataLibrary;
using MSOI.Models;
using MSOI.Repositories;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace MSOI.Services
{
    public class ItemReleaseService : IItemReleaseService
    {

        private readonly IItemReleaseRepository _repository;

        public ItemReleaseService(IItemReleaseRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<ItemReleaseModel>> GetItemReleases(int? id = null, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null)
        {
            List<ItemReleaseModel> itemReleaseModels = await _repository.GetItemReleases(id, worker_id, item_type_id, size, color, release_date, exchange_date);

            if (itemReleaseModels == null || itemReleaseModels.Count == 0)
            {
                throw new Exception("Wystąpił błąd podczas pobierania danych z bazy na temat przedmiotów i ich właścicieli");
            }

            return itemReleaseModels;

        }

        public Task<bool> InsertReleaseItem(ItemReleaseModel itemType)
        {
            ValidateId(itemType.Id);
            ValidateId(itemType.Worker_id);
            ValidateId(itemType.Item_type_id);
            ValidateSize(itemType.Size);
            ValidateColor(itemType.Color);
            ValidateDates(itemType.Release_date, itemType.Exchange_date);

            return _repository.InsertReleaseItem(itemType);
        }

        public Task<bool> UpdateItemRelease(int id, int? worker_id = null, int? item_type_id = null, string? size = null, string? color = null, DateTime? release_date = null, DateTime? exchange_date = null)
        {
            ValidateId(id);
            if(worker_id != null) ValidateId(worker_id);
            if (item_type_id != null)  ValidateId(item_type_id);
            if (size != null)  ValidateSize(size);
            if (color != null)  ValidateColor(color);
            if (release_date.HasValue && exchange_date.HasValue) ValidateDates(release_date, exchange_date);

            return _repository.UpdateItemRelease(id, worker_id, item_type_id, size, color, release_date, exchange_date);
        }

        public async Task<bool> DeleteData(int id)
        {
            bool succes = await _repository.DeleteData(id);
            if (!succes) throw new InvalidOperationException("Nie udało sie usunąć powiązania między przedmiotem a przacownikiem.");
            return succes;
        }

        private void ValidateId(int? id)
        {
            if (!id.HasValue || id <= 0)
            {
                throw new ArgumentException("Id musi być liczbą dodatnią.");
            }
        }

        private void ValidateSize(string size)
        {
            if (!string.IsNullOrEmpty(size) && !Regex.IsMatch(size, "^[a-zA-Z0-9]{1,10}$"))
            {
                throw new ArgumentException("Rozmiar może zawierać tylko litery i cyfry i mieć maksymalnie 10 znaków.");
            }
        }

        private void ValidateColor(string color)
        {
            if (!string.IsNullOrEmpty(color) && !Regex.IsMatch(color, "^[a-zA-Z\\s-]{1,20}$"))
            {
                throw new ArgumentException("Kolor może zawierać tylko litery, spacje i myślniki, maksymalnie 20 znaków.");
            }

        }

        private void ValidateDates(DateTime? release_date, DateTime? exchange_date)
        {
            if (release_date.HasValue && release_date > DateTime.Today)
            {
                throw new ArgumentException("Data wydania nie może być w przyszłości.");
            }

            if (exchange_date.HasValue && exchange_date > DateTime.Today)
            {
                throw new ArgumentException("Data wymiany nie może być w przyszłości.");
            }

            if (release_date.HasValue && exchange_date.HasValue && exchange_date < release_date)
            {
                throw new ArgumentException("Data wymiany nie może być wcześniejsza niż data wydania.");
            }

        }


    }

}
