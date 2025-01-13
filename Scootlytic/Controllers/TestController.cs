using Microsoft.AspNetCore.Mvc;
using Scootlytic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Scootlytic.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Tenta consultar algo no banco de dados, por exemplo, todas as entradas na tabela "Users"
            var users = _context.Users.ToList();

            if (users.Count > 0)
            {
                return Ok("Conexão ao banco de dados bem-sucedida!");
            }
            else
            {
                return BadRequest("Não foram encontrados usuários na base de dados.");
            }
        }
    }
}
