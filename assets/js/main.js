// Seleciona os botões no cabeçalho
const cartButton = document.querySelector('.cart-button');
const backButton = document.querySelector('.back-button');

// Evento para redirecionar ao clicar no carrinho
if (cartButton) {
    cartButton.addEventListener('click', () => {
        window.location.href = 'cart.html';
    });
}

// Evento para redirecionar ao clicar na seta de voltar
if (backButton) {
    backButton.addEventListener('click', () => {
        window.location.href = 'login.html'; 
    });
}

document.querySelectorAll('.product-colors').forEach(group => {
    group.addEventListener('click', event => {
        if (event.target.classList.contains('color-box')) {
            // Remove a seleção de todos os quadrados no grupo
            group.querySelectorAll('.color-box').forEach(box => box.classList.remove('selected'));
            
            // Adiciona a seleção ao quadrado clicado
            event.target.classList.add('selected');
        }
    });
});
