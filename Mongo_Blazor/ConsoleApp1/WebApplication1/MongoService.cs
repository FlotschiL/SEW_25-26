using MongoDB.Bson;
using MongoDB.Driver;

public class DynamicMongoService
{
    private readonly IMongoDatabase _database;

    public DynamicMongoService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("YourDatabaseName");
    }

    public async Task<List<Dictionary<string, object>>> GetCollectionDataAsync(string collectionName)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var documents = await collection.Find(_ => true).ToListAsync();

        // Convert BsonDocument to Dictionary for easier API serialization
        return documents.Select(doc => 
            doc.ToDictionary(el => el.Name, el => BsonTypeMapper.MapToDotNetValue(el.Value))
        ).ToList();
    }
}