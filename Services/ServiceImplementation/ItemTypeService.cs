using Dapper;
using DataLibrary;
using MSOI.Models;
using MSOI.Repositories;
using MSOI.Repositories.RepositoryImplementations;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MSOI.Services.ServiceImplementation
{
    public class ItemTypeService : IItemTypeService
    {

        private readonly IItemTypeRepository _repository;
        public ItemTypeService(IItemTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ItemTypeModel>> GetItemTypes(int? id = null, string? item_name = null, int? default_replacement_period = null, string? item_description = null)
        {
            List<ItemTypeModel> itemTypeModels = await _repository.GetItemTypes(id, item_name, default_replacement_period, item_description);

            if (itemTypeModels == null || itemTypeModels.Count == 0)
            {
                throw new Exception("Błąd podczas pobierania przedmiotów z bazy.");
            }

            return itemTypeModels;
        }

        public Task<bool> InsertData(ItemTypeModel itemType)
        {
            ValidateItemName(itemType.Item_name);
            ValidateItemDescription(itemType.Item_description);
            ValidateDefaultReplacementPeriod(itemType.Default_replacement_period);

            return _repository.InsertData(itemType);
        }


        public Task<bool> UpdateData(int id, string? item_name = null, int? default_replacement_period = null, string? item_description = null)
        {
            if (item_name != null) ValidateItemName(item_name);
            if (item_description != null) ValidateItemDescription(item_description);
            if (default_replacement_period != null) ValidateDefaultReplacementPeriod(default_replacement_period);

            return _repository.UpdateData(id, item_name, default_replacement_period, item_description);
        }

        public async Task<bool> DeleteData(ItemTypeModel itemType)
        {
            bool success = await _repository.DeleteData(itemType);

            if (!success)
            {
                throw new InvalidOperationException("Wystąpił błąd podczas usuwania przedmiotu z bazy.");
            }
            return success;
        }

        private bool ValidateItemName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Nazwa przedmiotu musi zostać wprowadzona");
            if (!Regex.IsMatch(name, @"^[A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż][A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż0-9 ]{0,49}$"))
            {
                throw new Exception("Nazwa przedmiotu może zawierać jedynie litery, cyfry oraz być nie dłuższa niż 50 znaków.");
            }
            return true;
        }

        private bool ValidateDefaultReplacementPeriod(int? default_replacement_period)
        {
            if (default_replacement_period == null) throw new ArgumentNullException("Podaj domyśly okres zwrotu przedmiotu w dniach");
            if (default_replacement_period <= 0)
            {
                throw new ArgumentOutOfRangeException("Czas zwrotu przedmiotu nie może być mniejszy bądź równy zero.");
            }
            return true;
        }

        private bool ValidateItemDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description), "Opis przedmiotu musi zostać wprowadzony.");

            if (description.Length > 100)
            {
                throw new ArgumentException("Opis przedmiotu może zawierać znaki specjalne, liczby, litery i nie może być dłuższy niż 100 znaków.");
            }
            return true;
        }




    }

}