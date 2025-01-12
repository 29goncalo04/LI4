using Microsoft.AspNetCore.Mvc;

namespace Scootlytic.Scooter
{
    public class ScooterController : Controller
    {
        // Método para renderizar a página de Login
        public IActionResult Scooter()
        {
            return View();
        }
        public IActionResult StepsSpeedy()
        {
            return View();
        }
        public IActionResult StepsGlidy()
        {
            return View();
        }
    }
}