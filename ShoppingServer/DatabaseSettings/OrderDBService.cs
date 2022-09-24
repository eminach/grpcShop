using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ShoppingServer.DatabaseSettings
{
    public class OrderDBService
    {
        private readonly IMongoCollection<BusinessLogic.Models.Order> _ordersCollection;

        public OrderDBService(
            IOptions<MongoDatabaseSettings> mongoDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                mongoDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDatabaseSettings.Value.DatabaseName);

            _ordersCollection = mongoDatabase.GetCollection<BusinessLogic.Models.Order>(
                mongoDatabaseSettings.Value.OrderCollectionName);
        }

        public async Task<List<BusinessLogic.Models.Order>> GetAsync() =>
            await _ordersCollection.Find(_ => true).ToListAsync();

        public async Task<BusinessLogic.Models.Order?> GetAsync(string id) =>
            await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BusinessLogic.Models.Order newOrder) =>
            await _ordersCollection.InsertOneAsync(newOrder);

        public async Task UpdateAsync(string id, BusinessLogic.Models.Order updateOrder) =>
            await _ordersCollection.ReplaceOneAsync(x => x.Id == id, updateOrder);

        public async Task RemoveAsync(string id) =>
            await _ordersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
