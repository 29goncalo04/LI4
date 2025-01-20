using Microsoft.AspNetCore.Mvc;
using Scootlytic.Models;
using Scootlytic.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Scootlytic.Cart
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Paypal()
        {
            return View();
        }
        public IActionResult Mbway()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }



        [HttpGet]
        public IActionResult GetCartItems()
        {
            var userEmail = Request.Headers["User-Email"].ToString();
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { success = false, message = "Carrinho não encontrado." });
            }

            var itensCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes,
                    a => a.IdTrotinete,
                    t => t.IdTrotinete,
                    (a, t) => new { Adicionada = a, Trotinete = t }
                )
                .ToList();
            return Json(new { success = true, items = itensCarrinho });
        }


        [HttpGet]
        public IActionResult RemoveFromCart(string modelo)
        {
            var userEmail = Request.Headers["User-Email"].ToString();
            Console.WriteLine(modelo);

            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { error = "Carrinho não encontrado." });
            }

            var itens = _context.Adicionada
                .Join(
                    _context.Trotinetes,
                    a => a.IdTrotinete,
                    t => t.IdTrotinete,
                    (a, t) => new { Adicionada = a, Trotinete = t }
                )
                .Where(a => a.Adicionada.IdCarrinho == carrinho.IdCarrinho && a.Trotinete.Modelo == modelo)
                .ToList();

            if (itens.Count == 0)
            {
                Console.WriteLine($"Nenhuma trotinete com o modelo {modelo} encontrada no carrinho.");
                return Json(new { error = "Nenhuma trotinete do modelo especificado encontrada no carrinho." });
            }
            _context.Adicionada.RemoveRange(itens.Select(i => i.Adicionada));
            _context.SaveChanges();
            var trotineteIds = itens.Select(i => i.Trotinete.IdTrotinete).Distinct().ToList();
            var trotinetesNaoAssociadas = _context.Adicionada
                .Any(a => trotineteIds.Contains(a.IdTrotinete));
            if (!trotinetesNaoAssociadas)
            {
                var trotinetesARemover = _context.Trotinetes
                    .Where(t => trotineteIds.Contains(t.IdTrotinete))
                    .ToList();

                _context.Trotinetes.RemoveRange(trotinetesARemover);
                _context.SaveChanges();
            }
            var itemsCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes,
                    a => a.IdTrotinete,
                    t => t.IdTrotinete,
                    (a, t) => new { Trotinete = t }
                )
                .ToList();

            decimal newTotalPrice = itemsCarrinho
                .Sum(item => item.Trotinete.Modelo == "SPEEDY Electric Scooter" ? 79.99m : item.Trotinete.Modelo == "GLIDY Scooter" ? 49.99m : 0);

            carrinho.ValorTotal = newTotalPrice;
            _context.SaveChanges();

            return Json(new { newTotalPrice });
        }

        [HttpPost]
        public IActionResult FinalizarCompra([FromBody] Dictionary<string, string> requestData)        {
            var userEmail = Request.Headers["User-Email"].ToString();
            requestData.TryGetValue("MetodoPagamento", out string metodoPagamento);

            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { error = "Carrinho não encontrado." });
            }

            var trotinetesNoCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Select(a => a.Trotinete)
                .ToList();

            if (trotinetesNoCarrinho.Count == 0)
            {
                return Json(new { error = "O carrinho está vazio." });
            }

            var novaEncomenda = new Encomenda
            {
                DataEntrega = DateTime.UtcNow,
                MetodoPagamento = metodoPagamento,
                Condicao = 1,
                EmailUtilizador = userEmail
            };

            _context.Encomendas.Add(novaEncomenda);
            _context.SaveChanges();

            foreach (var trotinete in trotinetesNoCarrinho)
            {
                trotinete.NumeroEncomenda = novaEncomenda.Numero;
            
                var passosNecessarios = trotinete.Modelo switch
                {
                    "GLIDY Scooter" => new List<(int passo, string peca)>
                    {
                        (7, "Frames"),
                        (8, "Wheels")
                    },
                    "SPEEDY Electric Scooter" => new List<(int passo, string peca)>
                    {
                        (1, "Frames"),
                        (2, "Motors"),
                        (3, "Batteries"),
                        (4, "Control Screens"),
                        (5, "Wheels"),
                        (5, "Brakes"),
                        (6, "Lights")
                    },
                    _ => new List<(int passo, string peca)>()
                };
            
                foreach (var (idPasso, nomePeca) in passosNecessarios)
                {
                    int quantidadeNecessaria = (nomePeca == "Lights" || nomePeca == "Wheels") ? 2 : 1;
            
                    for (int i = 0; i < quantidadeNecessaria; i++)
                    {
                        var pecaDisponivel = _context.Pecas
                            .FirstOrDefault(p => p.Nome == nomePeca && p.Estado == 0);
            
                        if (pecaDisponivel == null)
                        {
                            var novasPecas = Enumerable.Range(0, 10)
                                .Select(_ => new Peca { Nome = nomePeca, Estado = 0 })
                                .ToList();
                            _context.Pecas.AddRange(novasPecas);
                            _context.SaveChanges();
            
                            pecaDisponivel = novasPecas.First();
                        }
            
                        var existePassoPeca = _context.PassoPeca
                            .AsNoTracking()
                            .Any(pp => pp.PassoId == idPasso && pp.PecaReferencia == pecaDisponivel.Referencia);
            
                        if (!existePassoPeca)
                        {
                            var passoPeca = new PassoPeca
                            {
                                PassoId = idPasso,
                                PecaReferencia = pecaDisponivel.Referencia
                            };
                            _context.PassoPeca.Add(passoPeca);
                        }
            
                        pecaDisponivel.Estado = 1;
            
                        _context.Pecas.Attach(pecaDisponivel);
                        _context.SaveChanges();
                    }
                }
            }
            _context.SaveChanges();
            var itensAdicionados = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .ToList();

            _context.Adicionada.RemoveRange(itensAdicionados);
            carrinho.ValorTotal = 0;

            _context.SaveChanges();
            return Json(new { success = true, message = "Pagamento efetuado e encomenda criada." });
        }
    }
}