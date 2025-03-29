using Dapper;
using DataLibrary;
using MSOI.Components.Pages;
using MSOI.Models;
using MSOI.Repositories;
using MySql.Data.MySqlClient;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MSOI.Services
{
    public class WorkerService
    {

        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

    }
}
