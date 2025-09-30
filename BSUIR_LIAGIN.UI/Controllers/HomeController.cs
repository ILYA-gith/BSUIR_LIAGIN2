using BSUIR_LIAGIN.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR_LIAGIN.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly UriData _uriData;

        public HomeController(UriData uriData)
        {
            _uriData = uriData;
        }

        // Главная страница
        public IActionResult Index()
        {
            // Передаём StaticUri в ViewBag, чтобы картинки грузились
            ViewBag.StaticUri = _uriData.StaticUri;
            return View();
        }

        // Дополнительная страница "О проекте"
        public IActionResult About()
        {
            ViewBag.StaticUri = _uriData.StaticUri;
            return View();
        }

        // Дополнительная страница "Контакты"
        public IActionResult Contact()
        {
            ViewBag.StaticUri = _uriData.StaticUri;
            return View();
        }
    }
}
