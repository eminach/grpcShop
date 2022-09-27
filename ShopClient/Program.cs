using System.Threading.Tasks;
using Grpc.Net.Client;
using ShopClient;
using BusinessLogic.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        // The port number must match the port of the gRPC server.
        using var channel = GrpcChannel.ForAddress("https://localhost:7062");
        var stockClient = new ShopClient.Stock.StockClient(channel);
        var orderClient = new ShopClient.Order.OrderClient(channel);

        //HINT: use this ONLY when needs to populate product and order data
        //stockClient.PopulateProducts(new ProductRequest());

        BusinessLogic.Models.Order newOrder = new BusinessLogic.Models.Order()
        {
            ProdID = "63336671e4302f944737fcf7",
            Quantity = 2
        };

        //Check prod availablity in stock
        var stockResponse = stockClient.AvailableInStock(new StockRequest() { ProductId = newOrder.ProdID, Quantity = newOrder.Quantity });
        Console.WriteLine($"The product with ID: {0} is available for '2' quantity: {stockResponse.QuantityAvailable} "); //TODO beautify message

        //Make order
        if(stockResponse.QuantityAvailable)
        {
            //After Payment successfull

            //Try decrease amount from stock
            var sellResponse = stockClient.TakeFromStock(new StockRequest() { ProductId = newOrder.ProdID, Quantity = newOrder.Quantity });
            if(sellResponse.Succes)
            {
                //Sell was possible case

                //Perform order
                var orderResponse = orderClient.Order(new OrderRequest() { Product = newOrder.ProdID, Quantity = newOrder.Quantity });
                if(orderResponse.Succes)
                {
                    Console.WriteLine("Order succesfully implemented");
                }
                else
                {
                    Console.WriteLine("Negative result message here");
                }
            }
            else
            {
                Console.Write("Negative results message here");
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}