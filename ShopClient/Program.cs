using System.Threading.Tasks;
using Grpc.Net.Client;
using ShopClient;

internal class Program
{
    private static void Main(string[] args)
    {
        // The port number must match the port of the gRPC server.
        using var channel = GrpcChannel.ForAddress("https://localhost:7062");
        var stockClient = new Stock.StockClient(channel);
        var orderClient = new Order.OrderClient(channel);

        //HINT: use this ONLY when needs to populate product and order data
        //stockClient.PopulateProducts(new ProductRequest());

        var stockResponse = stockClient.AvailableInStock(new StockRequest() { ProductId = "632f92d907653e20c924c83a", Quantity = 2 });
        Console.WriteLine($"The product with ID: {0} is available for '2' quantity: {stockResponse.QuantityAvailable} "); //TODO beautify message

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}