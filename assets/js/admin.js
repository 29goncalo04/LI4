document.querySelector('#check-users').addEventListener('click', function(event) {
    event.preventDefault();
    window.location.href = 'users.html';
  });

document.querySelector('#check-parts').addEventListener('click', function(event) {
  event.preventDefault();
  window.location.href = 'parts.html';
});

// Seleciona o botão de voltar pelo seletor de classe
const backButton = document.querySelector('.back-button');

// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = 'login.html';
});