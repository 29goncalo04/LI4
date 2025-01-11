// Seleciona o botão de voltar pelo seletor de classe
const backButton = document.querySelector('.back-button');

// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = 'users_packages.html';
});