using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;

namespace BSUIR_LIAGIN.API.Services
{
    public interface ICategoryService
    {
        /// <summary> 
        /// Получение списка всех категорий 
        /// </summary> 
        /// <returns></returns> 
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
