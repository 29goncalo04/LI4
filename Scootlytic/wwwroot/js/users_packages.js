// Função para redirecionar para a página do modelo Speedy
function goToSpeedyPage() {
    window.location.href = '../../pages/stepsSpeedy.html';
}

// Função para redirecionar para a página do modelo Glidy
function goToGlidyPage() {
    window.location.href = '../../pages/stepsGlidy.html';
}


// Seleciona o botão de voltar pelo seletor de classe
const backButton = document.querySelector('.back-button');

// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = 'main.html';
});