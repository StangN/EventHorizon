using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IDataService
    {
        public void SaveData(Entry entry);

        public string GetData(string id);
    }
}
