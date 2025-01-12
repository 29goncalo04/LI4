using Microsoft.AspNetCore.Mvc;

namespace Scootlytic.Main
{
    public class MainController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}