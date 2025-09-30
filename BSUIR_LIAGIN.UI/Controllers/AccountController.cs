using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpPost]
    public IActionResult LogOut()
    {
        HttpContext.SignOutAsync(); 

        return RedirectToAction("Index", "Home");
    }
}
