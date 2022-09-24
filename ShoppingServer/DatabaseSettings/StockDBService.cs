using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ShoppingServer.DatabaseSettings
{
    public class StockDBService
    {
        private readonly IMongoCollection<BusinessLogic.Models.Stock> _booksCollection;

        public StockDBService(
            IOptions<MongoDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<BusinessLogic.Models.Stock>(
                bookStoreDatabaseSettings.Value.StockCollectionName);
        }

        public async Task<List<BusinessLogic.Models.Stock>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<BusinessLogic.Models.Stock?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<BusinessLogic.Models.Stock?> GetByProductAsync(string productID) =>
           await _booksCollection.Find(x => x.ProdID == productID).FirstOrDefaultAsync();

        public async Task CreateAsync(BusinessLogic.Models.Stock newStock) =>
            await _booksCollection.InsertOneAsync(newStock);

        public async Task UpdateAsync(string? id, BusinessLogic.Models.Stock updateStock) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateStock);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
