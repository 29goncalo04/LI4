using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;  // Adiciona o namespace da sua aplicação
using Scootlytic.Models; // Adiciona o namespace do seu modelo User
using Microsoft.EntityFrameworkCore;
using System.Linq; // Para utilizar LINQ

namespace Scootlytic.Scooter
{
    public class ScooterController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ScooterController(ApplicationDbContext context)
        {
            _context = context;
        }
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