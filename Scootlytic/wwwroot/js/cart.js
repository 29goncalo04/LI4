// Seleciona todos os ícones de lixo
const trashIcons = document.querySelectorAll('.trash-icon');

// Seleciona o ícone de seta
const backArrow = document.querySelector('.back-arrow');
const paypal = document.querySelector('.paypal');
const mbway = document.querySelector('.mbway');

trashIcons.forEach(icon => {
    icon.addEventListener('click', () => {
        // Acede ao elemento pai mais próximo com a classe 'product'
        const product = icon.closest('.product');
        if (product) {
            // Remove o elemento da página
            product.remove();
        }
    });
});

if (backArrow) {
    backArrow.addEventListener('click', () => {
        // Redireciona para uma página temporária
        window.location.href = '/Main/Main';
    });
}

if (paypal) {
    paypal.addEventListener('click', () => {
        // Redireciona para uma página temporária
        window.location.href = '/Cart/Paypal';
    });
}

if (mbway) {
    mbway.addEventListener('click', () => {
        // Redireciona para uma página temporária
        window.location.href = '/Cart/Mbway';
    });
}