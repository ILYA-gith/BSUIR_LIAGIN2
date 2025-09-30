using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.ProductService;

namespace BSUIR_LIAGIN.UI.Areas.Admin.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

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
    }
}
