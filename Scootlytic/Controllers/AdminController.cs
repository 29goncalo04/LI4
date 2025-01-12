using Microsoft.AspNetCore.Mvc;

namespace Scootlytic.Controllers
{
    public class AdminController : Controller
    {
        // Action que renderiza a página principal do Admin
        public IActionResult Admin()
        {
            return View();
        }

        // Action para renderizar a página de peças
        public IActionResult Parts()
        {
            return View();
        }

        public IActionResult UsersList()
        {
            return View();
        }
    }
}
