// Seleciona todos os elementos de step
const steps = document.querySelectorAll('.step');

// Adiciona um evento de clique para cada step
steps.forEach(step => {
  step.addEventListener('click', () => {
    // Remove a classe 'active' de todos os steps
    steps.forEach(s => s.classList.remove('active'));
    
    // Adiciona a classe 'active' ao step clicado
    step.classList.add('active');
  });
});
