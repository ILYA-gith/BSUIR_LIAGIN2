using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.ProductService;

namespace BSUIR_LIAGIN.UI.Areas.Admin.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var resp = await _productService.GetProductByIdAsync(id.Value);
            if (resp.Successfull && resp.Data != null)
            {
                Book = resp.Data;
                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            await _productService.DeleteProductAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
