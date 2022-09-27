using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection.Metadata;

namespace ShoppingServer.DatabaseSettings
{
    /// <summary>
    /// Decorator class
    /// </summary>
    public class StockDBService
    {
        private readonly IMongoCollection<BusinessLogic.Models.Stock> _stocksCollection;

        public StockDBService(
            IOptions<MongoDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _stocksCollection = mongoDatabase.GetCollection<BusinessLogic.Models.Stock>(
                bookStoreDatabaseSettings.Value.StockCollectionName);
        }

        public async Task<List<BusinessLogic.Models.Stock>> GetAsync() =>
            await _stocksCollection.Find(_ => true).ToListAsync();

        public async Task<BusinessLogic.Models.Stock?> GetAsync(string id) =>
            await _stocksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<BusinessLogic.Models.Stock?> GetByProductAsync(string productID) =>
           await _stocksCollection.Find(x => x.ProdID == productID).FirstOrDefaultAsync();

        public async Task CreateAsync(BusinessLogic.Models.Stock newStock) =>
            await _stocksCollection.InsertOneAsync(newStock);

        public async Task UpdateAsync(string? id, BusinessLogic.Models.Stock updateStock)
        {
            ReplaceOneResult result;
            long currentTS = updateStock.Timestamp;
            updateStock.Timestamp = DateTimeOffset.UtcNow.Ticks;

            FilterDefinition<BusinessLogic.Models.Stock> filter = Builders<BusinessLogic.Models.Stock>.Filter.Eq(r => r.Id, updateStock.Id)
            & Builders<BusinessLogic.Models.Stock>.Filter.Eq(r => r.Timestamp, currentTS);

            result = await _stocksCollection.ReplaceOneAsync(filter: filter, updateStock, new ReplaceOptions
            {
                IsUpsert = false
            }).ConfigureAwait(false);

            if (!result.IsAcknowledged)
            {
                throw new Exception("Data is not written");
            }
            if (result.ModifiedCount == 0 && _stocksCollection.CountDocuments(r => r.Id == updateStock.Id) == 1)
            {
                throw new Exception("Concurency failed");
            }
        }

        public async Task RemoveAsync(string id) =>
            await _stocksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
