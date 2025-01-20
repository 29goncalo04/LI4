using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;
using Scootlytic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Scootlytic.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Renderizar Login
        public IActionResult Login()
        {
            return View();
        }

        // Processar dados Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == "admin" && password == "admin")
            {
                return RedirectToAction("Admin", "Admin");
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Utilizador ou senha inválidos.";
                return View();
            }


            return RedirectToAction("Main", "Main");
        }

        // Renderizar Register
        public IActionResult Register()
        {
            return View();
        }

        // Processar dados Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return BadRequest("Este email já está em uso. Por favor, escolha outro.");
            }
    
            if (password.Length < 9)
            {
                return BadRequest("A senha precisa de ter pelo menos 9 caracteres.");
            }
        
            var newCarrinho = new Carrinho();
            _context.Carrinhos.Add(newCarrinho);
            await _context.SaveChangesAsync();
    
            var newUser = new User
            {
                Email = email,
                Password = password,
                CartId = newCarrinho.IdCarrinho
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok("Registro bem-sucedido!");
        }
    }
}
