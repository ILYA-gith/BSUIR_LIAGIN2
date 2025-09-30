using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using BSUIR_LIAGIN.UI.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BSUIR_LIAGIN.UI.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var cats = await _categoryService.GetCategoryListAsync();
            Categories = (cats.Successfull && cats.Data != null)
                ? new SelectList(cats.Data, "Id", "Name")
                : new SelectList(Enumerable.Empty<Category>(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Автоматически выставляем CategoryId из выбранной категории
            if (Book.Category != null && Book.Category.Id > 0)
            {
                Book.CategoryId = Book.Category.Id;
            }
            Book.Category = null;

            // Заполняем MIME тип изображения, если файл передан
            if (Image != null)
            {
                Book.ImageMimeType = Image.ContentType;
            }

            var resp = await _productService.CreateProductAsync(Book, Image);
            if (!resp.Successfull)
            {
                ModelState.AddModelError(string.Empty, resp.ErrorMessage ?? "Ошибка при создании книги");
                await OnGetAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
