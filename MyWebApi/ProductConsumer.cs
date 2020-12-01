using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MyWebApi
{
    public class ProductConsumer:IConsumer<Product>    
        
    {
        ILogger<ProductConsumer> _logger;

        public ProductConsumer(ILogger<ProductConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Product> context)
        {
            _logger.LogInformation("Product: {@Product}", context.Message);
        }
    }
}
