using Microsoft.AspNetCore.Mvc;

namespace Scootlytic.Cart
{
    public class CartController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Paypal()
        {
            return View();
        }
        public IActionResult Mbway()
        {
            return View();
        }
    }
}