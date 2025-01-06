document.querySelector("#login-form").addEventListener("submit", function(event) {
    event.preventDefault(); // Impede o envio padrão do formulário
  
    const email = document.querySelector('input[type="email"]').value;
    const password = document.querySelector('input[type="password"]').value;
  
    // Simulação de validação básica
    if (email === "user@example.com" && password === "password123") {
      alert("Login bem-sucedido!");
      window.location.href = "pagina-principal.html"; // Substituir pela página principal
    } else if(email === "admin" && password === "admin"){
      //fazer admin a entrar na app
    } else {
      alert("Email ou password incorretos!");
    }
});