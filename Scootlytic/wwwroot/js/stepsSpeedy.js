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
    instruction: "Assemble the frame and prepare for motor installation."
  },
  2: {
    image: "/images/SScooter1_2_3.png",
    parts: [
      { img: "/images/engine.png", number: 1, name: "Motor" },
    ],
    instruction: "Install the motor into the frame and secure it."
  },
  3: {
    image: "/images/SScooter1_2_3.png",
    parts: [
      { img: "/images/battery.png", number: 1, name: "Battery" }
    ],
    instruction: "Place the battery into the designated compartment."
  },
  4: {
    image: "/images/SScooter4.png",
    parts: [
      { img: "/images/controlScreens.png", number: 1, name: "Display" }
    ],
    instruction: "Install the display on the handlebars."
  },
  5: {
    image: "/images/SScooter5.png",
    parts: [
      { img: "/images/tires.png", number: 2, name: "Wheels" },
      { img: "/images/brakes.png", number: 1, name: "Brakes" }
    ],
    instruction: "Attach the wheels and brakes to the scooter."
  },
  6: {
    image: "/images/SpeedyScooter.png",
    parts: [
      { img: "/images/lights.png", number: 2, name: "Lights" }
    ],
    instruction: "Install the lights to finish the scooter assembly."
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
