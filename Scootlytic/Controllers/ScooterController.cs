using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;
using Scootlytic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public IActionResult StepsSpeedy(int? step)
        {
            ViewBag.Step = step;
            return View();
        }
        
        public IActionResult StepsGlidy(int? step)
        {
            ViewBag.Step = step;
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

            var newScooter = new Trotinete
            {
                Modelo = scooterInfo.Name,
                Cor = scooterInfo.Color,
            };

            try
            {
                _context.Trotinetes.Add(newScooter);
                await _context.SaveChangesAsync();
                List<Possui> possui = new List<Possui>();
    
                if (newScooter.Modelo == "SPEEDY Electric Scooter")
                {
                    possui.AddRange(new List<Possui>
                    {
                        new Possui { IdPasso = 1, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 2, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 3, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 4, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 5, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 6, IdTrotinete = newScooter.IdTrotinete }
                    });
                }
                else if (newScooter.Modelo == "GLIDY Scooter")
                {
                    possui.AddRange(new List<Possui>
                    {
                        new Possui { IdPasso = 7, IdTrotinete = newScooter.IdTrotinete },
                        new Possui { IdPasso = 8, IdTrotinete = newScooter.IdTrotinete }
                    });
                }

                _context.Possui.AddRange(possui);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao salvar a trotinete: " + ex.Message });
            }

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
    public string Name { get; set; }
    public string Color { get; set; }
    public decimal Price {get; set; }
}