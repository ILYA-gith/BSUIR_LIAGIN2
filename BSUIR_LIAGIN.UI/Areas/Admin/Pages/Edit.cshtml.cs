using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using BSUIR_LIAGIN.UI.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BSUIR_LIAGIN.UI.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Book Book { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var resp = await _productService.GetProductByIdAsync(id);
            if (!resp.Successfull || resp.Data == null)
            {
                return NotFound();
            }

            Book = resp.Data;

            var cats = await _categoryService.GetCategoryListAsync();
            Categories = (cats.Successfull && cats.Data != null)
                ? new SelectList(cats.Data, "Id", "Name", Book.CategoryId)
                : new SelectList(Enumerable.Empty<Category>(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(Book.Id);
                return Page();
            }

            // выставляем CategoryId из выбранной категории
            if (Book.Category != null && Book.Category.Id > 0)
            {
                Book.CategoryId = Book.Category.Id;
            }
            Book.Category = null;

            // если загружен новый файл — обновляем MIME
            if (Image != null)
            {
                Book.ImageMimeType = Image.ContentType;
            }

            await _productService.UpdateProductAsync(Book.Id, Book, Image);

            return RedirectToPage("./Index");
        }
    }
}
