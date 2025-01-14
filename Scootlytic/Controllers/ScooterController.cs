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

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] ScooterInfo scooterInfo)
        {
            if (scooterInfo == null)
            {
                return Json(new { success = false, message = "Informações inválidas." });
            }


            var userEmail = Request.Headers["User-Email"].ToString();

            // Obtém o utilizador atual (supondo que o email do utilizador está armazenado na sessão)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return Json(new { success = false, message = "Utilizador não encontrado." });
            }

            var cartId = user.CartId;

            var cart = await _context.Carrinhos
                .FirstOrDefaultAsync(c => c.IdCarrinho == cartId);
            cart.ValorTotal += Math.Round(scooterInfo.Price, 2);
            _context.Carrinhos.Update(cart);
            await _context.SaveChangesAsync();

            // Criar a nova trotinete
            var newScooter = new Trotinete
            {
                Modelo = scooterInfo.Name,
                Cor = scooterInfo.Color,
            };

            try
            {
                _context.Trotinetes.Add(newScooter);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao salvar a trotinete: " + ex.Message });
            }

            // Criar a entrada na tabela 'Adicionada'
            var addedItem = new Adicionada
            {
                IdCarrinho = cart.IdCarrinho,
                IdTrotinete = newScooter.IdTrotinete
            };

            try
            {
                _context.Adicionada.Add(addedItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao adicionar ao carrinho: " + ex.Message });
            }

            return Json(new { success = true });
        }
    }
}


public class ScooterInfo
{
    public string Name { get; set; }        // Nome do modelo da trotinete
    public string Color { get; set; }       // Cor da trotinete
    public decimal Price {get; set; }
}