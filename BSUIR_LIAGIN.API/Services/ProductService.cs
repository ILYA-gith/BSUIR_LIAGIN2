using BSUIR_LIAGIN.API.Data;
using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BSUIR_LIAGIN.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseData<Book>> CreateProductAsync(Book product)
        {
            try
            {
                _context.Books.Add(product);
                await _context.SaveChangesAsync();
                return ResponseData<Book>.Success(product);
            }
            catch (Exception ex)
            {
                return ResponseData<Book>.Error($"Ошибка при создании: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление книги по Id
        /// </summary>
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Получение книги по Id
        /// </summary>
        public async Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            try
            {
                var book = await _context.Books
                    .Include(b => b.Category)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return ResponseData<Book>.Error("Книга не найдена");

                // Склейка: добавляем базовый URL
                var request = _httpContextAccessor.HttpContext!.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                book.Image = string.IsNullOrEmpty(book.Image) ? null : $"{baseUrl}/{book.Image}";

                return ResponseData<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return ResponseData<Book>.Error($"Ошибка при получении: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение списка книг с фильтрацией и пагинацией
        /// </summary>
        public async Task<ResponseData<ListModel<Book>>> GetProductListAsync(
            string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(b => b.Category.NormalizedName == categoryNormalizedName);
            }

            var count = await query.CountAsync();
            var listModel = new ListModel<Book>();

            if (count == 0)
                return ResponseData<ListModel<Book>>.Success(listModel);

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Book>>.Error("No such page");

            listModel.Items = await query
                .OrderBy(b => b.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Склейка URL для каждой книги
            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            foreach (var book in listModel.Items)
            {
                if (!string.IsNullOrEmpty(book.Image))
                    book.Image = $"{baseUrl}/{book.Image}";
            }

            listModel.CurrentPage = pageNo;
            listModel.TotalPages = totalPages;

            return ResponseData<ListModel<Book>>.Success(listModel);
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(int id, Book product)
        {
            try
            {
                var existing = await _context.Books.FindAsync(id);
                if (existing == null) return;

                // Обновляем только нужные поля
                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.CategoryId = product.CategoryId;

                // Если пришёл новый Image — обновляем (относительный путь!)
                if (!string.IsNullOrEmpty(product.Image))
                {
                    existing.Image = product.Image;
                    existing.ImageMimeType = product.ImageMimeType;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении книги: {ex.Message}", ex);
            }
        }
    }
}
