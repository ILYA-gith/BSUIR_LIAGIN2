using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewComponents;

public class CartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {

        var html = "00,0 руб <i class='fa-solid fa-cart-shopping'></i> (0)";
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
}