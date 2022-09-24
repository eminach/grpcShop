using BusinessLogic.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ShoppingServer.DatabaseSettings
{
    public class ProductDBService
    {
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductDBService(
            IOptions<MongoDatabaseSettings> mongoDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                mongoDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDatabaseSettings.Value.DatabaseName);

            _productsCollection = mongoDatabase.GetCollection<Product>(
                mongoDatabaseSettings.Value.ProductCollectionName);
        }

        public async Task<List<Product>> GetAsync() =>
            await _productsCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _productsCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updateProduct) =>
            await _productsCollection.ReplaceOneAsync(x => x.Id == id, updateProduct);

        public async Task RemoveAsync(string id) =>
            await _productsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
