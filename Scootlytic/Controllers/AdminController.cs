using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;
using Scootlytic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Scootlytic.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult PartsInstalled()
        {
            return View();
        }


        public async Task<IActionResult> Parts()
        {
            var pecasTodosNomes = new[] { "Batteries", "Brakes", "Lights", "Wheels", "Motors", "Control Screens", "Frames" };

            var pecasQuantidades = await _context.Pecas
                .Where(p => p.Estado == 0)
                .GroupBy(p => p.Nome)
                .Select(g => new
                {
                    Nome = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();
        
            var pecasQuantidadeDict = pecasTodosNomes
                .ToDictionary(
                    nome => nome, 
                    nome => pecasQuantidades.FirstOrDefault(p => p.Nome == nome)?.Quantidade ?? 0
                );
        
            return View(pecasQuantidadeDict);
        }

        public IActionResult UsersList()
        {
            return View();
        }

        public IActionResult UsersPackages()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUserOrders()
        {
            var userEmail = Request.Headers["User-Email"].ToString();

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
                        .Select(t => new 
                        {
                            t.IdTrotinete,
                            t.Modelo,
                            Quantidade = 1
                        })
                        .ToList()
                })
                .ToList();

            return Json(encomendas);
        }


        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new List<string>());
            }
        
            var users = await _context.Users
                .Where(u => u.Email.StartsWith(query))
                .Select(u => u.Email)
                .ToListAsync();
        
            return Json(users);
        }

        public IActionResult UsersPackagesAdmin()
        {
            return View();
        }


        public async Task<IActionResult> GetOrderDetails(string numero)
        {
            int orderId;
            int.TryParse(numero, out orderId);
            var trotinetes = _context.Trotinetes
                .Where(t => t.NumeroEncomenda == orderId)
                .ToList();

            if (!trotinetes.Any())
            {
                return NotFound();
            }

            var detalhesTrotinetes = new List<object>();

            foreach (var trotinete in trotinetes)
            {
                var passoAtual = _context.Possui
                    .Where(p => p.IdTrotinete == trotinete.IdTrotinete)
                    .OrderBy(p => p.IdPasso)
                    .Select(p => p.IdPasso)
                    .FirstOrDefault();

                if (passoAtual != 0){
                    var numeroPasso = _context.Passos
                        .Where(p => p.idPasso == passoAtual)
                        .Select(p => p.NumeroPasso)
                        .FirstOrDefault();
                    detalhesTrotinetes.Add(new
                    {
                        TrotineteId = trotinete.IdTrotinete,
                        PassoAtual = numeroPasso
                    });
                }
                else{
                    detalhesTrotinetes.Add(new
                    {
                        TrotineteId = trotinete.IdTrotinete,
                        PassoAtual = passoAtual
                    });
                }
            }
            return Json(detalhesTrotinetes);
        }
    }
}