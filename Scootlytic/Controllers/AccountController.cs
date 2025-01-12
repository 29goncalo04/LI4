using Microsoft.AspNetCore.Mvc;

namespace Scootlytic.Controllers
{
    public class AccountController : Controller
    {
        // Método para renderizar a página de Login
        public IActionResult Login()
        {
            return View();
        }

        // Método para processar os dados do formulário de Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin" && password == "admin")
            {
                return RedirectToAction("Admin", "Admin");
            }
            else
            {
                return RedirectToAction("Main", "Main");
            }
        }

        // Método para renderizar a página de Register
        public IActionResult Register()
        {
            return View();
        }

        // Método para processar os dados do formulário de Register
        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            if (password.Length < 9)
            {
                // Se a senha for muito curta, retorna um erro
                return Content("A senha precisa ter pelo menos 9 caracteres.");
            }
        
            // Lógica para criar o usuário (substitua com a lógica de base de dados real)
            ViewBag.SuccessMessage = "Registration successful! You can now log in.";
            
            // Redireciona para a página de login após o sucesso
            return RedirectToAction("Login");
        }
    }
}
