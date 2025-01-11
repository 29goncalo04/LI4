// Seleciona todos os elementos de step
const steps = document.querySelectorAll('.step');

// Seleciona o elemento de imagem
const stepImage = document.getElementById('step-image');

// Imagens para cada passo
const stepImages = {
  1: "../assets/images/SScooter1_2_3.png",
  2: "../assets/images/SScooter1_2_3.png",
  3: "../assets/images/SScooter1_2_3.png",
  4: "../assets/images/SScooter4.png",
  5: "../assets/images/SScooter5.png",
  6: "../assets/images/SpeedyScooter.png"
};

// Adiciona um evento de clique para cada step
steps.forEach(step => {
  step.addEventListener('click', () => {
    // Remove a classe 'active' de todos os steps
    steps.forEach(s => s.classList.remove('active'));
    
    // Adiciona a classe 'active' ao step clicado
    step.classList.add('active');
    
    // Pega o número do passo
    const stepNumber = step.getAttribute('data-step');
    
    // Atualiza a imagem com base no passo
    stepImage.src = stepImages[stepNumber];
  });
});
