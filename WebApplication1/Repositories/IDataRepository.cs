using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IDataRepository
    {
        public void SaveEntry(Entry entry);
    }
}
