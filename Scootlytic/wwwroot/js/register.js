document.querySelector("form").addEventListener("submit", function(event) {
    event.preventDefault(); // Impede o envio padrão do formulário
  
    const email = document.querySelector('input[type="email"]').value;
    const password = document.querySelector('input[type="password"]').value;
  
    if (password.length < 9) {
      alert("A senha precisa ter pelo menos 9 caracteres!");
    } else {      
      // Redirecionar para outra página
      window.location.href = "../../pages/login.html"; // Substituir pelo link desejado
    }
  });
  