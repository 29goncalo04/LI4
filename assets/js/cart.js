// Seleciona todos os ícones de lixo
const trashIcons = document.querySelectorAll('.trash-icon');

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
