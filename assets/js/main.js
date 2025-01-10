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

// Função para mapear RGB para nomes de cores
function rgbToColorName(rgb) {
    const colors = {
        "rgb(0, 0, 0)": "Black",
        "rgb(204, 204, 204)": "Grey",
        // Adicione mais cores aqui, se necessário
    };
    return colors[rgb] || "Unknown Color";
}

document.querySelectorAll('.details-button').forEach(button => {
    button.addEventListener('click', event => {
        const productElement = event.target.closest('.product');
        const productDetails = productElement.querySelector('.product-details');
        const colorGroup = productDetails.querySelector('.product-colors');
        const productName = productElement.querySelector('.product-name').textContent.trim();
        const productPrice = productDetails.querySelector('.product-price').textContent.trim();
        const productReference = productElement.getAttribute('data-reference');
        const selectedColorBox = colorGroup.querySelector('.color-box.selected');

        if (!selectedColorBox) {
            alert("Por favor, selecione uma cor antes de ver os detalhes.");
        } else {
            const colorRgb = getComputedStyle(selectedColorBox).backgroundColor;
            const colorName = rgbToColorName(colorRgb);
            const url = `scooter.html?name=${encodeURIComponent(productName)}&price=${encodeURIComponent(productPrice)}&color=${encodeURIComponent(colorName)}&reference=${productReference}`;
            window.location.href = url;
        }
    });
});
