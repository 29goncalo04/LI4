const backButton = document.querySelector('.back-button');

backButton.addEventListener('click', () => {
    window.location.href = '/Admin/Admin';
});

document.getElementById("search-input").addEventListener("input", function() {
    let query = this.value.trim();

    if (query.length === 0) {
        document.getElementById("search-results").innerHTML = "";
        return;
    }

    fetch(`/Admin/search?query=${query}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Erro na requisição");
            }
            return response.json();
        })
        .then(data => {
            const resultsContainer = document.getElementById("search-results");
            resultsContainer.innerHTML = '';

            if (data.length === 0) {
                resultsContainer.innerHTML = "<p>No users found</p>";
                return;
            }

            resultsContainer.style.display = 'block';

            data.forEach(user => {
                const div = document.createElement('div');
                div.classList.add('result-item');
                div.textContent = user;
                div.addEventListener('click', () => {
                    sessionStorage.setItem('userEmail', user);
                    window.location.href = "/Admin/UsersPackagesAdmin";
                });
                resultsContainer.appendChild(div);
            });
            
        })
        .catch(error => {
            console.error("Erro ao buscar utilizadores:", error);
        });
});
