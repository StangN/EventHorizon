using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _dataRepository;

        public DataService()
        {
            _dataRepository = new DataRepository();
        }
        public void SaveData(Entry entry)
        {
            _dataRepository.SaveEntry(entry);
        }

        public string GetData(string id)
        {
            return "data";
        }
    }
}
