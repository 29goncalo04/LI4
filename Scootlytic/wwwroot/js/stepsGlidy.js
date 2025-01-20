const steps = document.querySelectorAll('.step');

const stepImage = document.getElementById('step-image');

const stepParts = document.getElementById('step-parts');

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

const urlParams = new URLSearchParams(window.location.search);
const initialStep = urlParams.get('step') ? parseInt(urlParams.get('step')) : 1;

function updateStep(stepNumber) {
  steps.forEach(s => s.classList.remove('active'));
  const activeStep = document.querySelector(`.step[data-step="${stepNumber}"]`);
  if (activeStep) {
    activeStep.classList.add('active');
  }

  const details = stepDetails[stepNumber];
  stepImage.src = details.image;
  document.getElementById('step-number').textContent = stepNumber;
  document.getElementById('step-instruction').textContent = details.instruction;
  stepParts.innerHTML = '';
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
updateStep(initialStep);
steps.forEach(step => {
  step.addEventListener('click', () => {
    const stepNumber = parseInt(step.getAttribute('data-step'));
    updateStep(stepNumber);
  });
});
document.querySelector(".back-button").addEventListener("click", () => {
  window.location.href = "/Admin/UsersPackages";
});
