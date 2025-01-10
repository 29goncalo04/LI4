// Seleciona o botão de voltar pelo seletor de classe
const backButton = document.querySelector('.close-button');
const addButton = document.querySelector('.add-to-cart');


// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = 'main.html';
});

addButton.addEventListener('click', () => {
    alert("Produto adicionado ao carrinho.");
});