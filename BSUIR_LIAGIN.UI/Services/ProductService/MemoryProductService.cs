using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using Microsoft.AspNetCore.Http;

namespace BSUIR_LIAGIN.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private readonly ICategoryService _categoryService;
        private readonly List<Book> _books = new();
        private readonly int _itemsPerPage;

        public MemoryProductService(IConfiguration config, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _itemsPerPage = config.GetValue<int>("ItemsPerPage"); // читаем из appsettings.json
            SetupData();
        }


        public Task<ResponseData<Book>> CreateProductAsync(Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return Task.FromResult(ResponseData<Book>.Error("Книга не найдена"));

            return Task.FromResult(ResponseData<Book>.Success(book));
        }

        public Task<ResponseData<ListModel<Book>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            IEnumerable<Book> query = _books;

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(b => b.Category != null &&
                                         b.Category.NormalizedName == categoryNormalizedName);
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);

            var items = query
                .Skip((pageNo - 1) * _itemsPerPage)
                .Take(_itemsPerPage)
                .ToList();

            var listModel = new ListModel<Book>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Task.FromResult(ResponseData<ListModel<Book>>.Success(listModel));
        }

        public Task UpdateProductAsync(int id, Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        // Метод для начального наполнения книгами
        private void SetupData()
        {
            var categories = _categoryService.GetCategoryListAsync().Result.Data!;

            _books.AddRange(new List<Book>
            {
                       new Book
        {
            Id = 1,
            Name = "1984",
            Description = "Антиутопия Джорджа Оруэлла",
            CategoryId = categories.First(c => c.NormalizedName == "dystopia").Id,
            Category = categories.First(c => c.NormalizedName == "dystopia"),
            Price = 15.99m,
            Image = "Images/1984.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 2,
            Name = "Горе от ума",
            Description = "Классическая пьеса Александра Грибоедова",
            CategoryId = categories.First(c => c.NormalizedName == "classical-drama").Id,
            Category = categories.First(c => c.NormalizedName == "classical-drama"),
            Price = 9.99m,
            Image = "Images/woe_from_mind.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 3,
            Name = "Война и мир",
            Description = "Исторический роман Льва Толстого",
            CategoryId = categories.First(c => c.NormalizedName == "historical-novels").Id,
            Category = categories.First(c => c.NormalizedName == "historical-novels"),
            Price = 25.50m,
            Image = "Images/war_and_peace.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 4,
            Name = "Гарри Поттер",
            Description = "Фэнтези роман Дж. К. Роулинг",
            CategoryId = categories.First(c => c.NormalizedName == "fantasy").Id,
            Category = categories.First(c => c.NormalizedName == "fantasy"),
            Price = 18.00m,
            Image = "Images/harry_potter.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 5,
            Name = "Три мушкетера",
            Description = "Приключенческий роман Александра Дюма",
            CategoryId = categories.First(c => c.NormalizedName == "adventure").Id,
            Category = categories.First(c => c.NormalizedName == "adventure"),
            Price = 14.00m,
            Image = "Images/three_musketeers.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 6,
            Name = "Шерлок Холмс",
            Description = "Детективные истории Артура Конан Дойла",
            CategoryId = categories.First(c => c.NormalizedName == "detective").Id,
            Category = categories.First(c => c.NormalizedName == "detective"),
            Price = 12.00m,
            Image = "Images/sherlock.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 7,
            Name = "Мастер и Маргарита",
            Description = "Магический реализм Михаила Булгакова",
            CategoryId = categories.First(c => c.NormalizedName == "magical-realism").Id,
            Category = categories.First(c => c.NormalizedName == "magical-realism"),
            Price = 20.00m,
            Image = "Images/master_and_margarita.jpg",
            ImageMimeType = "image/jpeg"
        },
        new Book
        {
            Id = 8,
            Name = "Преступление и наказание",
            Description = "Роман Фёдора Достоевского",
            CategoryId = categories.First(c => c.NormalizedName == "russian-classics").Id,
            Category = categories.First(c => c.NormalizedName == "russian-classics"),
            Price = 16.00m,
            Image = "Images/crime_and_punishment.jpg",
            ImageMimeType = "image/jpeg"
        }
            });

        }
    }
}
