using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;

namespace BSUIR_LIAGIN.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Антиутопия", NormalizedName = "dystopia" },
                new Category { Id = 2, Name = "Классическая драма", NormalizedName = "classical-drama" },
                new Category { Id = 3, Name = "Исторические романы", NormalizedName = "historical-novels" },
                new Category { Id = 4, Name = "Фэнтези", NormalizedName = "fantasy" },
                new Category { Id = 5, Name = "Приключения", NormalizedName = "adventure" },
                new Category { Id = 6, Name = "Детектив", NormalizedName = "detective" },
                new Category { Id = 7, Name = "Магический реализм", NormalizedName = "magical-realism" },
                new Category { Id = 8, Name = "Русская классика", NormalizedName = "russian-classics" }
            };

            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
