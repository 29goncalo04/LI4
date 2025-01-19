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
            // Nomes de todas as peças esperadas
            var pecasTodosNomes = new[] { "Batteries", "Brakes", "Lights", "Wheels", "Motors", "Control Screens", "Frames" };
        
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
        
            // Cria um dicionário para armazenar a quantidade de cada peça, preenchendo 0 para as peças ausentes
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
                        .Select(t => new 
                        {
                            t.IdTrotinete,    // Incluindo o ID da trotinete
                            t.Modelo,         // Modelo da trotinete
                            Quantidade = 1 // Contando a quantidade por modelo
                        })
                        .ToList()
                })
                .ToList();

            return Json(encomendas); // Retorna o JSON com os detalhes das trotinetes
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

        //int orderId;
        //int.TryParse(numero, out orderId);



        public async Task<IActionResult> GetOrderDetails(string numero)
        {
            int orderId;
            int.TryParse(numero, out orderId);
            // Buscando as trotinetes associadas à encomenda
            var trotinetes = _context.Trotinetes
                .Where(t => t.NumeroEncomenda == orderId)
                .ToList();

            if (!trotinetes.Any())
            {
                return NotFound();
            }

            // Lógica para calcular o passo atual de cada trotinete
            var detalhesTrotinetes = new List<object>();

            foreach (var trotinete in trotinetes)
            {
                var passoAtual = _context.Possui
                    .Where(p => p.IdTrotinete == trotinete.IdTrotinete)
                    .OrderBy(p => p.IdPasso) // Ordena pelo IdPasso (do menor para o maior)
                    .Select(p => p.IdPasso) // Seleciona o IdPasso
                    .FirstOrDefault(); // Retorna o menor valor ou 0 se não houver nenhum

                if (passoAtual != 0){
                    var numeroPasso = _context.Passos
                        .Where(p => p.idPasso == passoAtual)
                        .Select(p => p.NumeroPasso)
                        .FirstOrDefault();
                    // Adiciona os detalhes da trotinete e seu passo atual
                    detalhesTrotinetes.Add(new
                    {
                        TrotineteId = trotinete.IdTrotinete, // ID da trotinete
                        PassoAtual = numeroPasso // Passo atual
                    });
                }
                else{
                    // Adiciona os detalhes da trotinete e seu passo atual
                    detalhesTrotinetes.Add(new
                    {
                        TrotineteId = trotinete.IdTrotinete, // ID da trotinete
                        PassoAtual = passoAtual // Passo atual
                    });
                }
            }
            // Retorna os detalhes da encomenda e as trotinetes com o passo atual como JSON
            return Json(detalhesTrotinetes); // Resposta no formato JSON
        }
    }
}