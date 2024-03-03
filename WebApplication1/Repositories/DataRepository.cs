using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class DataRepository : IDataRepository
    {
        const string connnectionString = "placeholder";

        IMongoClient _client;

        IMongoCollection<Entry> entries;

        public void SaveEntry(Entry entry) {
            try
            {
                _client = new MongoClient(connnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                return;
            }

            try
            {
                var collection = _client.GetDatabase("testDB").GetCollection<Entry>("entries");
                collection.InsertOne(entry);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");
                Console.WriteLine(e);
                Console.WriteLine();
                return;
            }
        }

    }
}
