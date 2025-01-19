// Seleciona todos os elementos de step
const steps = document.querySelectorAll('.step');

// Seleciona o elemento de imagem
const stepImage = document.getElementById('step-image');

// Seleciona o elemento onde as partes serão exibidas
const stepParts = document.getElementById('step-parts');

// Mapeamento das imagens e partes associadas a cada passo
const stepDetails = {
  1: {
    image: "/images/glidyStep1.png",
    parts: [
      { img: "/images/frames.png", number: 1, name: "Frame" },
    ],
    instruction: "Assemble the frame and deck"
  },
  2: {
    image: "/images/GlidyScooter.png",
    parts: [
      { img: "/images/tires.png", number: 2, name: "Wheels" }
    ],
    instruction: "Install the wheels."
  }
};

// Verifica o parâmetro `step` na URL
const urlParams = new URLSearchParams(window.location.search);
const initialStep = urlParams.get('step') ? parseInt(urlParams.get('step')) : 1;

// Função para atualizar o passo
function updateStep(stepNumber) {
  // Remove a classe 'active' de todos os steps
  steps.forEach(s => s.classList.remove('active'));
  
  // Adiciona a classe 'active' ao step correspondente
  const activeStep = document.querySelector(`.step[data-step="${stepNumber}"]`);
  if (activeStep) {
    activeStep.classList.add('active');
  }

  // Atualiza a imagem e as partes com base no passo
  const details = stepDetails[stepNumber];
  stepImage.src = details.image; // Atualiza a imagem
  document.getElementById('step-number').textContent = stepNumber; // Atualiza o número do passo
  document.getElementById('step-instruction').textContent = details.instruction; // Atualiza a instrução
  
  // Limpa as partes anteriores
  stepParts.innerHTML = '';
  
  // Adiciona as partes associadas ao passo
  details.parts.forEach(part => {
    const partElement = document.createElement('div');
    partElement.classList.add('part');
    
    partElement.innerHTML = `
      <div class="image-large">
        <img src="${part.img}" alt="${part.name}">
      </div>
      <div class="label">
        <span class="part-number">${part.number}</span>
        <span class="part-name">${part.name}</span>
      </div>
    `;
    
    stepParts.appendChild(partElement);
  });
}

// Inicializa o passo com base na URL ou padrão
updateStep(initialStep);

// Adiciona um evento de clique para cada step
steps.forEach(step => {
  step.addEventListener('click', () => {
    const stepNumber = parseInt(step.getAttribute('data-step'));
    updateStep(stepNumber);
  });
});

// Função para voltar à página principal
document.querySelector(".back-button").addEventListener("click", () => {
  window.location.href = "/Admin/UsersPackages";
});
