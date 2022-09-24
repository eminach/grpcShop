namespace BusinessLogic.Models
{
    public class MockData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name="Prod 1"
                },
                new Product()
                {
                    Name="Prod 2"
                },
                new Product()
                {
                    Name="Prod 3"
                }
            };
        }

        public static List<Stock> InitializeStock()
        {
            var products = new List<Stock>();
            products.Add(new Stock() { Quantity = 15, OperationDate = DateTime.Now.AddDays(-5) });
            products.Add(new Stock() { Quantity = 8, OperationDate = DateTime.Now.AddDays(-5) });
            products.Add(new Stock() { Quantity = 11, OperationDate = DateTime.Now.AddDays(-4) });

            return products;
        }
    }
}
