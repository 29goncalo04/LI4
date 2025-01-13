const backButton = document.querySelector('.back-button');

// Adiciona um evento de clique ao botão
backButton.addEventListener('click', () => {
    window.location.href = '/Admin/Admin';
});

document.getElementById("search-input").addEventListener("input", function() {
    let query = this.value.trim(); // Pega o valor digitado no campo de pesquisa

    // Se não houver texto, limpa os resultados
    if (query.length === 0) {
        document.getElementById("search-results").innerHTML = "";
        return;
    }

    // Faz a requisição ao servidor com o parâmetro "query"
    fetch(`/Admin/search?query=${query}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Erro na requisição");
            }
            return response.json(); // Resposta como JSON
        })
        .then(data => {
            // Limpa os resultados antigos
            const resultsContainer = document.getElementById("search-results");
            resultsContainer.innerHTML = '';

            // Se não houver resultados, mostrar mensagem
            if (data.length === 0) {
                resultsContainer.innerHTML = "<p>No users found</p>";
                return;
            }

            resultsContainer.style.display = 'block';

            // Cria e exibe os resultados da pesquisa
            data.forEach(user => {
                const div = document.createElement('div');
                div.classList.add('result-item');  // Altere 'search-item' para 'result-item'
                div.textContent = user;  // Supondo que 'user' tenha uma propriedade 'email'
                resultsContainer.appendChild(div);
            });
            
        })
        .catch(error => {
            console.error("Erro ao buscar utilizadores:", error);
        });
});
