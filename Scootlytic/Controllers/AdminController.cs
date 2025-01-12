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
    }
}
