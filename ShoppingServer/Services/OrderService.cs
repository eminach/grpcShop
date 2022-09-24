using Grpc.Core;
using ShoppingServer.DatabaseSettings;

namespace ShoppingServer.Services
{
    public class OrderService : Order.OrderBase
    {
        private readonly ILogger<OrderService> _logger;
        private readonly OrderDBService _orderDBService;

        public OrderService(ILogger<OrderService> logger, OrderDBService orderDBService)
        {
            _logger = logger;
            _orderDBService = orderDBService;
        }

        public override async Task<OrderResponse> Order(OrderRequest request, ServerCallContext context)
        {
            BusinessLogic.Models.Order newOrder = new()
            {
                ProdID = request.Product,
                Quantity = request.Quantity
            };
            
            await _orderDBService.CreateAsync(newOrder);

            return await Task.FromResult(new OrderResponse() { Product = request.Product, Succes = true });
        }
    }
}
