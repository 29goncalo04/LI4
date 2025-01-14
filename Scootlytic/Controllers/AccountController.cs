using Microsoft.AspNetCore.Mvc;
using Scootlytic.Data;  // Adiciona o namespace da sua aplicação
using Scootlytic.Models; // Adiciona o namespace do seu modelo User
using Microsoft.EntityFrameworkCore;
using System.Linq; // Para utilizar LINQ

namespace Scootlytic.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para renderizar a página de Login
        public IActionResult Login()
        {
            return View();
        }

        // Método para processar os dados do formulário de Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Se o utilizador for administrador
            if (email == "admin" && password == "admin")
            {
                return RedirectToAction("Admin", "Admin"); // Página de administração, caso seja o admin
            }
            // Verifica se o utilizador existe na base de dados
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            // Se o utilizador não for encontrado ou a senha estiver errada
            if (user == null)
            {
                ViewBag.ErrorMessage = "Utilizador ou senha inválidos.";
                return View(); // Retorna à página de login com uma mensagem de erro
            }


            // Caso contrário, redireciona para a página principal (Main)
            return RedirectToAction("Main", "Main");
        }

        // Método para renderizar a página de Register
        public IActionResult Register()
        {
            return View();
        }

        // Método para processar os dados do formulário de Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (password.Length < 9)
            {
                // Se a senha for muito curta, retorna um erro
                return Content("A senha precisa ter pelo menos 9 caracteres.");
            }

            var newCarrinho = new Carrinho();
            _context.Carrinhos.Add(newCarrinho);
            await _context.SaveChangesAsync();

            // Cria um novo objeto User com os dados recebidos do formulário
            var newUser = new User
            {
                Email = email,  // Você pode usar outro campo se necessário
                Password = password,
                CartId = newCarrinho.IdCarrinho
            };

            // Adiciona o novo utilizador na base de dados
            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();
        
            // Lógica para criar o utilizador (substitua com a lógica de base de dados real)
            ViewBag.SuccessMessage = "Registration successful! You can now log in.";
            
            // Redireciona para a página de login após o sucesso
            return RedirectToAction("Login");
        }
    }
}
