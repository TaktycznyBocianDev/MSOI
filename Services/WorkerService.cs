using Dapper;
using DataLibrary;
using MSOI.Components.Pages;
using MSOI.Models;
using MSOI.Repositories;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MSOI.Services
{
    public class WorkerService : IWorkerService
    {

        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<List<WorkerModel>> GetWorkers(int? id = null, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {

            List<WorkerModel> workers = await _workerRepository.GetWorkers(id, name, surname, position, employment_date, pesel);

            if (workers == null)
            {
                throw new Exception("Lista pracowników nie mogła zostać pobrana → wystąpił błąd podczas pobierania listy pracowników.");
            }
            if (workers.Count == 0)
            {
                throw new Exception("Baza danych nie posiada pracowników → skontaktuj się z dostawcą aplikacji.");
            }

            return workers;

        }

        public Task<bool> InsertWorker(WorkerModel worker)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWorker(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteWorker(int id)
        {
            bool succed = await _workerRepository.DeleteData(id);

            if (!succed)
            {
                throw new Exception("Wystąpił błąd podczas usuwania pracownika. Operacja została anulowana.");
            }
            return succed;
        }


        private void ValidateName(string name)
        {
            if (name == null || name == string.Empty)
            {
                throw new ArgumentNullException("Imie pracownika nie zostało wprowadzone");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49}$"))
            {
                throw new Exception("Imie musi zaczynać się wielką literą, zawierać tylko litery (dopuszcza się polskie znaki) oraz maksymalnie długie na 50 znaków.");
            }
        }

        private void ValidateSurname(string surname)
        {
            if (surname == null || surname == string.Empty)
            {
                throw new ArgumentNullException("Imie pracownika nie zostało wprowadzone");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(surname, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49}(-[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49})?$"))
            {
                throw new Exception("Nazwisko musi zaczynać się wielką literą, zawierać tylko litery (dopuszcza się polskie znaki oraz myślik dla nazwisk dwuczłonowych) oraz może być maksymalnie długie na 50 znaków.");
            }
        }

        private void ValidatePosition(string position)
        {
            if (position == null || position == string.Empty)
            {
                throw new ArgumentNullException("Pozycja pracownika musi zostać wprowadzona");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(position, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż ]{1,49}$"))
            {
                throw new Exception("Pozycja pracownika nie może zawierać cyfr ani znakow specjalncych innych poza spacją.");
            }
        }

        private void ValidateEmploymentDate(DateTime employment_date)
        {
            if (employment_date > DateTime.Now)
            {
                throw new InvalidOperationException("Data zatrudnienia nie może być w przyszłości.");
            }

        }

        private static bool ValidatePesel(string pesel)
        {
            string peselPattern = @"^\d{11}$";
            if (!Regex.IsMatch(pesel, peselPattern))
            {
                return false;
            }

            int[] peselDigits = pesel.Select(c => c - '0').ToArray();

            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += peselDigits[i] * weights[i];
            }

            int controlDigit = (10 - (sum % 10)) % 10;

            return controlDigit == peselDigits[10];
        }


    }
}
