// Função para carregar as encomendas do usuário
async function loadOrders() {
    try {
        const response = await fetch('/Admin/GetUserOrders', {
            method: 'GET',
            headers: {
                'User-Email': sessionStorage.getItem('userEmail'),
                'Content-Type': 'application/json'
            }
        });

        // Verifique se a resposta é válida
        if (!response.ok) {
            throw new Error('Erro ao carregar as encomendas');
        }

        const data = await response.json();

        // Verifique se os dados são válidos
        if (data && Array.isArray(data)) {
            displayOrders(data);
        } else {
            throw new Error('Dados inválidos recebidos');
        }

    } catch (error) {
        console.error('Error loading orders:', error);
        alert('Erro ao carregar as encomendas. Tente novamente mais tarde.');
    }
}

// Função para exibir as encomendas na página
// Função para exibir as encomendas na página
// Função para formatar a data
function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa em 0
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

// Função para exibir as encomendas na página
function displayOrders(orders) {
    const ordersContainer = document.querySelector('#orders-container'); // Alterado para buscar pelo ID

    if (ordersContainer) { // Verifica se o contêiner foi encontrado
        orders.forEach(order => {
            // Calculando o preço total das trotinetes
            const totalPrice = order.trotinetes.reduce((total, t) => {
                if (t.modelo === 'SPEEDY Electric Scooter') {
                    return total + (79.99 * t.quantidade);
                } else if (t.modelo === 'GLIDY Scooter') {
                    return total + (49.99 * t.quantidade);
                }
                return total;
            }, 0);

            // Criação do elemento da ordem
            const orderElement = document.createElement('div');
            orderElement.classList.add('order');
            orderElement.innerHTML = `
                <div class="order-header">
                    <div class="package-box">
                        <div class="package">Package #${order.numero}</div>
                    </div>
                    <div class="price-box">
                        <div class="price">Price:<br>${totalPrice.toFixed(2)}€</div> <!-- Preço total -->
                    </div>
                    <div class="contents-box">
                        <div class="contents-title">Contents:</div>
                        <div class="contents">
                            <!-- SPEEDY à esquerda -->
                            ${order.trotinetes.filter(t => t.modelo === 'SPEEDY Electric Scooter').map(t => `
                                <div class="speedy">
                                    ${t.modelo} x${t.quantidade}
                                </div>
                            `).join('')}
                            <!-- GLIDY à direita -->
                            ${order.trotinetes.filter(t => t.modelo === 'GLIDY Scooter').map(t => `
                                <div class="glidy">
                                    ${t.modelo} x${t.quantidade}
                                </div>
                            `).join('')}
                        </div>
                    </div>                       
                    <div class="status-box">
                        <div class="status">Package status:<br>${order.condicao}</div>
                    </div>
                    <div class="delivery-box">
                        <div class="delivery">Delivery date:<br>${formatDate(order.dataEntrega)}</div>
                    </div>
                </div>
                
                <!-- Detalhes da encomenda (Tabela com "Step Number") -->
                <div class="order-details">
                    <table>
                        <thead>
                            <tr>
                                <th>Model</th>
                                <th>Step Number</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Para cada trotinete, criar uma linha na tabela -->
                            ${order.trotinetes.map(t => `
                                ${Array.from({ length: t.quantidade }).map(() => `
                                    <tr>
                                        <td onclick="goToPage('${t.modelo}')">${t.modelo}</td>
                                        <td>-</td> <!-- Usando "-" em vez da quantidade -->
                                    </tr>
                                `).join('')}
                            `).join('')}
                        </tbody>
                    </table>
                </div>        
            `;
            ordersContainer.appendChild(orderElement);
        });
    } else {
        console.error("Contêiner de encomendas não encontrado.");
    }
}

function goToPage(model) {
    // Função para navegar para a página detalhada do modelo de trotinete
    alert("A navegar para a página da " + model);
}








// Chamar a função para carregar as encomendas assim que a página for carregada
document.addEventListener('DOMContentLoaded', loadOrders);


// Função para voltar à página principal
document.querySelector(".back-button").addEventListener("click", () => {
    window.location.href = "/Main/Main";
});