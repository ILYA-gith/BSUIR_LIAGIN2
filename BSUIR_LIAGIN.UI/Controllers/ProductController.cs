using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.ProductService;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR_LIAGIN.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UriData _uriData;

        public ProductController(IProductService productService, ICategoryService categoryService, UriData uriData)
        {
            _productService = productService;
            _categoryService = categoryService;
            _uriData = uriData;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var categoriesResp = await _categoryService.GetCategoryListAsync();
            if (!categoriesResp.Successfull)
                return NotFound(categoriesResp.ErrorMessage);

            var categories = categoriesResp.Data!;

            var productsResp = await _productService.GetProductListAsync(category, pageNo);
            if (!productsResp.Successfull)
                return NotFound(productsResp.ErrorMessage);

            ViewData["currentCategory"] = category == null
                ? "Все категории"
                : categories.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Категория";
            ViewBag.Categories = categories;
            ViewBag.ApiUri = _uriData.ApiUri;       // для сервисов (JSON)
            ViewBag.StaticUri = _uriData.StaticUri; // для картинок
                                                    

            return View(productsResp.Data);
        }
    }
}
