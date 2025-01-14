using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;  // Adiciona o namespace da sua aplicação
using Scootlytic.Models; // Adiciona o namespace do seu modelo User
using Microsoft.EntityFrameworkCore;
using System.Linq; // Para utilizar LINQ


namespace Scootlytic.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
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

        public IActionResult UsersPackages()
        {
            return View();
        }


        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new List<string>());
            }
        
            // Buscar os utilizadores que começam com o texto da pesquisa (ignorando maiúsculas/minúsculas)
            var users = await _context.Users
                .Where(u => u.Email.StartsWith(query))
                .Select(u => u.Email) // Apenas o email será retornado
                .ToListAsync();
        
            return Json(users); // Retorna os emails como um array JSON
        }
    }
}
