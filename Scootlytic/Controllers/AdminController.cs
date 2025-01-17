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
        public async Task<IActionResult> Parts()
        {
            // Agrupa as peças com Estado == 0 e conta a quantidade de cada tipo
            var pecasQuantidades = await _context.Pecas
                .Where(p => p.Estado == 0) // Filtra para considerar apenas as peças com Estado 0
                .GroupBy(p => p.Nome)
                .Select(g => new
                {
                    Nome = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();
    
            // Cria um dicionário para armazenar a quantidade de cada peça
            var pecasQuantidadeDict = pecasQuantidades.ToDictionary(p => p.Nome, p => p.Quantidade);
    
            return View(pecasQuantidadeDict);
        }

        public IActionResult UsersList()
        {
            return View();
        }

        public IActionResult UsersPackages()
        {
            return View();  // Agora carrega a View `userspackages.cshtml`
        }

        [HttpGet]
        public IActionResult GetUserOrders()
        {
            var userEmail = Request.Headers["User-Email"].ToString(); // Pega o e-mail do usuário autenticado
    
            var encomendas = _context.Encomendas
                .Where(e => e.EmailUtilizador == userEmail)
                .Select(e => new
                {
                    e.Numero,
                    e.DataEntrega,
                    e.MetodoPagamento,
                    e.Condicao,
                    Trotinetes = _context.Trotinetes
                        .Where(t => t.NumeroEncomenda == e.Numero)
                        .GroupBy(t => t.Modelo)
                        .Select(g => new { Modelo = g.Key, Quantidade = g.Count() })
                        .ToList()
                })
                .ToList();
    
            return Json(encomendas); // Agora só retorna JSON quando chamado via fetch()
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

        public IActionResult UsersPackagesAdmin()
        {
            return View();
        }
    }
}
