using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;
using BSUIR_LIAGIN.UI.Services.ProductService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BSUIR_LIAGIN.UI.Areas.Admin.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public List<Book> Books { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            var resp = await _productService.GetProductListAsync(null, pageNo);
            if (resp.Successfull && resp.Data != null)
            {
                Books = resp.Data.Items;
                CurrentPage = resp.Data.CurrentPage;
                TotalPages = resp.Data.TotalPages;
            }
        }
    }
}
