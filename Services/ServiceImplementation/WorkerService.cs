using Dapper;
using DataLibrary;
using MSOI.Components.Pages;
using MSOI.Models;
using MSOI.Repositories;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MSOI.Services.ServiceImplementation
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

        public async Task<bool> InsertWorker(WorkerModel worker)
        {
            ValidateName(worker.Worker_name);
            ValidateSurname(worker.Worker_surname);
            ValidatePosition(worker.Position);
            ValidateEmploymentDate(worker.Employment_date);
            ValidatePesel(worker.Pesel);

            return await _workerRepository.InsertData(worker);
        }

        public async Task<bool> UpdateWorker(int id, string? name = null, string? surname = null, string? position = null, DateTime? employment_date = null, string? pesel = null)
        {
            if (name != null) ValidateName(name);
            if (surname != null) ValidateSurname(surname);
            if (position != null) ValidatePosition(position);
            if (employment_date != null) ValidateEmploymentDate(employment_date);
            if (pesel != null) ValidatePesel(pesel);

            return await _workerRepository.UpdateData(id, name, surname, position, employment_date, pesel);
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
            if (!Regex.IsMatch(name, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49}$"))
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
            if (!Regex.IsMatch(surname, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49}(-[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]{1,49})?$"))
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
            if (!Regex.IsMatch(position, @"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż ]{1,49}$"))
            {
                throw new Exception("Pozycja pracownika nie może zawierać cyfr ani znakow specjalncych innych poza spacją.");
            }
        }

        private void ValidateEmploymentDate(DateTime? employment_date)
        {
            if (!employment_date.HasValue)
            {
                throw new InvalidOperationException("Data zatrudnienia musi posiadać wartość.");

            }
            if (employment_date > DateTime.Now)
            {
                throw new InvalidOperationException("Data zatrudnienia nie może być w przyszłości.");
            }

        }

        private async Task ValidatePesel(string pesel)
        {
            if (string.IsNullOrWhiteSpace(pesel))
            {
                throw new ArgumentNullException(nameof(pesel), "PESEL nie może być pusty.");
            }

            if (!Regex.IsMatch(pesel, @"^\d{11}$"))
            {
                throw new ArgumentException("PESEL musi zawierać dokładnie 11 cyfr.", nameof(pesel));
            }

            int[] peselDigits = pesel.Select(c => c - '0').ToArray();
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += peselDigits[i] * weights[i];
            }

            int controlDigit = (10 - sum % 10) % 10;

            if (controlDigit != peselDigits[10])
            {
                throw new ArgumentException("Podano błędny PESEL – nie zgadza się cyfra kontrolna.", nameof(pesel));
            }

            bool doPeselExist = await _workerRepository.DoPeselExist(pesel);
            if (doPeselExist)
            {
                throw new InvalidOperationException("Pracownik z takim PESEL jest już zapisany w bazie.");
            }
        }



    }
}
