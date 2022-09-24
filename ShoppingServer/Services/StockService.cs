using BusinessLogic;
using Grpc.Core;
using ShoppingServer;
using ShoppingServer.DatabaseSettings;

namespace ShoppingServer.Services
{
    public class StockService : Stock.StockBase
    {
        private readonly ILogger<StockService> _logger;
        private readonly StockDBService _stockDBService;

        public StockService(ILogger<StockService> logger, StockDBService stockDBService)
        {
            _logger = logger;
            _stockDBService = stockDBService;
        }

        /// <summary>
        /// Service method to check product available quantity in stock
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="context">Service context</param>
        /// <returns></returns>
        public override async Task<StockAvailableResponse> AvailableInStock(StockRequest request, ServerCallContext context)
        {
            var result = await _stockDBService.GetByProductAsync(request.ProductId);

            if(result == null)
            {
                //TODO: Here may redesigned and return proper exception
                return await Task.FromResult(new StockAvailableResponse() { ProductId = request.ProductId, QuantityAvailable = false });
            }
            else
            {
                bool quantityAvailable = request.Quantity >= result.Quantity;
                return await Task.FromResult(new StockAvailableResponse() { ProductId = request.ProductId, QuantityAvailable = quantityAvailable });
            }
        }

        /// <summary>
        /// Service method to sell/take or decrease product available quantity in stock
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="context">Service context</param>
        /// <returns></returns>
        public override async Task<TakeFromStockResponse> TakeFromStock(StockRequest request, ServerCallContext context)
        {
            var result = await _stockDBService.GetByProductAsync(request.ProductId);//TODO: Check completed

            if (result == null)
            {
                //TODO: Exception
                return await Task.FromResult(new TakeFromStockResponse() { Product = request.ProductId, Succes = false });
            }
            else
            {
                int newQuantity = result.Quantity - request.Quantity;
                if (newQuantity < 0)
                {
                    //TODO: Exception
                    return await Task.FromResult(new TakeFromStockResponse() { Product = request.ProductId, Succes = false });
                }
                else
                {
                    BusinessLogic.Models.Stock updatedStock = new()
                    {
                        ProdID = request.ProductId,
                        Quantity = newQuantity,
                        OperationDate = DateTime.Now
                    };

                    await _stockDBService.UpdateAsync(result.Id, updatedStock); //TODO: Check completed

                    return await Task.FromResult(new TakeFromStockResponse() { Product = request.ProductId, Succes = true });
                }
            }
        }
    }
}
