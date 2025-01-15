using Microsoft.AspNetCore.Mvc;
using Scootlytic.Models; // Substitua pelo seu namespace
using Scootlytic.Data; // Substitua pelo seu contexto de banco de dados
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
            return View(); // A página inicial será carregada sem dados
        }



        [HttpGet]  // Ou [HttpPost] se preferir, mas o padrão é GET para essa operação
        public IActionResult GetCartItems()
        {
            var userEmail = Request.Headers["User-Email"].ToString(); // Pega o email do header

            // Busca o carrinho do usuário
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);

            if (carrinho == null)
            {
                return Json(new { success = false, message = "Carrinho não encontrado." });
            }

            // Agora fazemos o join com a tabela Trotinete
            var itensCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete, // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete, // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Adicionada = a, Trotinete = t } // Seleciona os dados que você precisa
                )
                .ToList();
            // Retorna os dados como JSON
            return Json(new { success = true, items = itensCarrinho });
        }


        [HttpGet]  // Usar GET para a remoção
        public IActionResult RemoveFromCart(string modelo)
        {
            var userEmail = Request.Headers["User-Email"].ToString(); // Pega o email do header
            Console.WriteLine(modelo);
        
            // Encontrar o carrinho do usuário
            var carrinho = _context.Carrinhos
                .FirstOrDefault(c => c.User.Email == userEmail);
        
            if (carrinho == null)
            {
                return Json(new { error = "Carrinho não encontrado." });
            }
        
            // Buscar todas as trotinetes do modelo especificado associadas ao carrinho
            var itens = _context.Adicionada
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete,  // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete,  // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Adicionada = a, Trotinete = t } // Seleciona dados de Adicionada e Trotinete
                )
                .Where(a => a.Adicionada.IdCarrinho == carrinho.IdCarrinho && a.Trotinete.Modelo == modelo)
                .ToList();
        
            if (itens.Count == 0)
            {
                Console.WriteLine($"Nenhuma trotinete com o modelo {modelo} encontrada no carrinho.");
                return Json(new { error = "Nenhuma trotinete do modelo especificado encontrada no carrinho." });
            }
        
            // Remover todas as trotinetes do modelo especificado da tabela Adicionada
            _context.Adicionada.RemoveRange(itens.Select(i => i.Adicionada));
            _context.SaveChanges();
        
            // Verificar se ainda existem trotinetes do modelo especificado em outros carrinhos
            var trotineteIds = itens.Select(i => i.Trotinete.IdTrotinete).Distinct().ToList();
            var trotinetesNaoAssociadas = _context.Adicionada
                .Any(a => trotineteIds.Contains(a.IdTrotinete));
        
            // Se não houverem mais registros de trotinetes associadas ao modelo, remover da tabela Trotinetes
            if (!trotinetesNaoAssociadas)
            {
                var trotinetesARemover = _context.Trotinetes
                    .Where(t => trotineteIds.Contains(t.IdTrotinete))
                    .ToList();
        
                _context.Trotinetes.RemoveRange(trotinetesARemover);
                _context.SaveChanges();
            }
        
            // Recalcular o total do carrinho após a remoção
            var itemsCarrinho = _context.Adicionada
                .Where(a => a.IdCarrinho == carrinho.IdCarrinho)
                .Join(
                    _context.Trotinetes, // Tabela Trotinete
                    a => a.IdTrotinete,  // Condição de join (IdTrotinete de Adicionada)
                    t => t.IdTrotinete,  // Condição de join (IdTrotinete de Trotinete)
                    (a, t) => new { Trotinete = t } // Seleciona apenas a Trotinete
                )
                .ToList();
        
            // Calcular o novo valor total
            decimal newTotalPrice = itemsCarrinho
                .Sum(item => item.Trotinete.Modelo == "SPEEDY Electric Scooter" ? 79.99m : item.Trotinete.Modelo == "GLIDY Scooter" ? 49.99m : 0);
        
            // Atualizar o valor total do carrinho na tabela Carrinho
            carrinho.ValorTotal = newTotalPrice;
            _context.SaveChanges();
            
            // Retorna o novo total
            return Json(new { newTotalPrice });
        }

    }
}