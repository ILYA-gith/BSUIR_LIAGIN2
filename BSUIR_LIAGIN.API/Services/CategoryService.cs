using BSUIR_LIAGIN.API.Data;
using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BSUIR_LIAGIN.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
