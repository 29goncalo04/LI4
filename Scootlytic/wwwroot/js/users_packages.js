// Função para redirecionar para a página do modelo Speedy
function goToSpeedyPage() {
    window.location.href = '/Scooter/StepsSpeedy';
}

// Função para redirecionar para a página do modelo Glidy
function goToGlidyPage() {
    window.location.href = '/Scooter/StepsGlidy';
}


// Seleciona o botão de voltar pelo seletor de classe
const backButton = document.querySelector('.back-button');

// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = '/Main/Main';
});