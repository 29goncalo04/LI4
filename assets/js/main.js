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

document.querySelectorAll('.details-button').forEach(button => {
    button.addEventListener('click', event => {
        // Encontra o grupo de cores correspondente ao botão clicado
        const productDetails = event.target.closest('.product-details');
        const colorGroup = productDetails.querySelector('.product-colors');
        
        // Verifica se alguma cor está selecionada
        const selectedColor = colorGroup.querySelector('.color-box.selected');

        if (!selectedColor) {
            alert("Por favor, selecione uma cor antes de ver os detalhes.");
        } else {
            // Aqui podes definir a navegação para a página de detalhes
            window.location.href = 'scooter.html'; // Altera para a página correta
        }
    });
});