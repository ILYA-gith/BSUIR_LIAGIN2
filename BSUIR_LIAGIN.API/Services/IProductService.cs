using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;

namespace BSUIR_LIAGIN.API.Services
{
    public interface IProductService
    {
        Task<ResponseData<ListModel<Book>>> GetProductListAsync(
            string? categoryNormalizedName,
            int pageNo = 1,
            int pageSize = 3);

        Task<ResponseData<Book>> GetProductByIdAsync(int id);
        Task UpdateProductAsync(int id, Book product);
        Task DeleteProductAsync(int id);
        Task<ResponseData<Book>> CreateProductAsync(Book product);
        Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
