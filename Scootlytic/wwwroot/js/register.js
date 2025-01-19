document.querySelector("form").addEventListener("submit", async function(event) {
  event.preventDefault(); // Impede o envio padrão do formulário

  // Coleta os valores dos campos de input
  const email = document.querySelector('input[type="email"]').value.trim();
  const password = document.querySelector('input[type="password"]').value.trim();

  // Verifica se a senha tem pelo menos 9 caracteres
  if (password.length < 9) {
    alert("A senha precisa ter pelo menos 9 caracteres!");
  } else {
    // Envia os dados para o servidor via POST
    const response = await fetch('/Account/Register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({ email, password })
    });

    if (response.ok) {
      // Se o registro for bem-sucedido, redireciona para a página de login
      alert("Ao criar conta aceita que a aplicação guarde o seu email e a sua password");
      window.location.href = '/Account/Login'; // Substitua pelo caminho correto se necessário
    } else {
      // Caso ocorra algum erro, exibe a mensagem
      const result = await response.text();
      alert(result); // Exibe a mensagem de erro do servidor (por exemplo, falha no registro)
    }
  }
});
