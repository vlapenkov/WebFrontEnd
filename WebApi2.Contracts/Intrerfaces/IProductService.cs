using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi2.Contracts.Dto;

namespace WebApi2.Contracts.Intrerfaces
{
    public interface IProductService
    {
        [Get("/api/products/getproducts")]
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
