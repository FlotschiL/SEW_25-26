using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace WebApplication1;
public class Dept 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
}
public class MongoService
{
    private readonly IMongoCollection<Dept> _products;

    public MongoService(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("YourDatabaseName");
        _products = database.GetCollection<Dept>("Products");
    }

    public async Task<List<Dept>> GetAsync() =>
        await _products.Find(_ => true).ToListAsync();
}
