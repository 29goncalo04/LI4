document.querySelector("form").addEventListener("submit", async function(event) {
  event.preventDefault();

  const email = document.querySelector('input[type="email"]').value.trim();
  const password = document.querySelector('input[type="password"]').value.trim();

  if (password.length < 9) {
    alert("A senha precisa de ter pelo menos 9 caracteres!");
  } else {
    const response = await fetch('/Account/Register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({ email, password })
    });

    if (response.ok) {
      alert("Ao criar conta, aceita que a aplicação guarde o seu email e a sua password.");
      window.location.href = '/Account/Login';
    } else {
      const result = await response.text();
      alert(result);
    }
  }
});
