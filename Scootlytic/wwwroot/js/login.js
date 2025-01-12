document.querySelector("form").addEventListener("submit", function(event) {
  event.preventDefault(); // Impede o envio padrão do formulário

  const email = document.querySelector('#email').value.trim();  // Captura o valor do email
  const password = document.querySelector('#password').value.trim();  // Captura o valor da senha

  // Verifica se o email e a senha são "admin"
  if (email === "admin" && password === "admin") {
    window.location.href = "../../pages/admin.html";  // Redireciona para admin.html
  } else {
    window.location.href = "../../pages/main.html";  // Redireciona para main.html
  }
});
