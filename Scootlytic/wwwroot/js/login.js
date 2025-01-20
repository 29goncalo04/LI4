document.querySelector("form").addEventListener("submit", async function(event) {
  event.preventDefault();

  const email = document.querySelector('#email').value.trim();  
  sessionStorage.setItem("userEmail", email);
  const password = document.querySelector('#password').value.trim();  
  const response = await fetch('/Account/Login', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: new URLSearchParams({ email, password })
  });

  if (response.redirected) {
      window.location.href = response.url;
  } else {
      document.body.innerHTML = await response.text();
  }
});
