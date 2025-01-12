document.querySelector("form").addEventListener("submit", async function(event) {
  event.preventDefault(); // Impede o envio padrão do formulário

  const email = document.querySelector('#email').value.trim();  
  const password = document.querySelector('#password').value.trim();  

  // Envia os dados para o servidor via POST
  const response = await fetch('/Account/Login', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({ email, password })
  });

  if (response.redirected) {
      // Redireciona o utilizador para a página que o servidor definiu
      window.location.href = response.url;
  } else {
      // Atualiza a página para mostrar a mensagem de erro (se existir)
      document.body.innerHTML = await response.text();
  }
});
