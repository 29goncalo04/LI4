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
