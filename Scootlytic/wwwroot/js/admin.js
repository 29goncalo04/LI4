document.querySelector('#check-users').addEventListener('click', function(event) {
    event.preventDefault();
    window.location.href = '/Admin/UsersList';
  });

document.querySelector('#check-parts').addEventListener('click', function(event) {
    event.preventDefault();
    window.location.href = '/Admin/Parts'; // Redireciona para a action Parts no AdminController
});


document.querySelector('.back-button').addEventListener('click', () => {
  window.location.href = '/Account/Login'; // Redirecionamento para a página de login
});
