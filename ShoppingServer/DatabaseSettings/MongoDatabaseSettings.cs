namespace ShoppingServer.DatabaseSettings
{
    public class MongoDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ProductCollectionName { get; set; } = null!;

        public string OrderCollectionName { get; set; } = null!;

        public string StockCollectionName { get; set; } = null!;
    }
}
