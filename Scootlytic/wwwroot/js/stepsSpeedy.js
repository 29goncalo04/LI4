// Seleciona todos os elementos de step
const steps = document.querySelectorAll('.step');

// Seleciona o elemento de imagem
const stepImage = document.getElementById('step-image');

// Seleciona o elemento onde as partes serão exibidas
const stepParts = document.getElementById('step-parts');

// Mapeamento das imagens e partes associadas a cada passo
const stepDetails = {
  1: {
    image: "/images/SScooter1_2_3.png",
    parts: [
      { img: "/images/frames.png", number: 1, name: "Frame" },
    ],
    instruction: "Assemble the frame and deck"
  },
  2: {
    image: "/images/SScooter1_2_3.png",
    parts: [
      { img: "/images/engine.png", number: 1, name: "Motor" },
    ],
    instruction: "Install the motor."
  },
  3: {
    image: "/images/SScooter1_2_3.png",
    parts: [
      { img: "/images/battery.png", number: 1, name: "Battery" }
    ],
    instruction: "Connect the battery."
  },
  4: {
    image: "/images/SScooter4.png",
    parts: [
      { img: "/images/controlScreens.png", number: 1, name: "Display" }
    ],
    instruction: "Set up the display."
  },
  5: {
    image: "/images/SScooter5.png",
    parts: [
      { img: "/images/tires.png", number: 2, name: "Wheels" },
      { img: "/images/brakes.png", number: 1, name: "Brakes" }
    ],
    instruction: "Install the wheels and braking system."
  },
  6: {
    image: "/images/SpeedyScooter.png",
    parts: [
      { img: "/images/lights.png", number: 2, name: "Lights" }
    ],
    instruction: "Add lights."
  }
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
    
    // Atualiza a imagem e as partes com base no passo
    const details = stepDetails[stepNumber];
    stepImage.src = details.image; // Atualiza a imagem
    document.getElementById('step-number').textContent = stepNumber; // Atualiza o número do passo
    

    // // Atualiza a instrução (isto tá a estragar o que tá à frentes)
    document.getElementById('step-instruction').textContent = details.instruction;
    
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
  });
});

// Função para voltar à página principal
document.querySelector(".back-button").addEventListener("click", () => {
  window.location.href = "/Admin/UsersPackages";
});