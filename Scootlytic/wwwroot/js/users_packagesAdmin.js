// Função para carregar as encomendas do usuário
async function loadOrders() {
    try {
        const userEmail = sessionStorage.getItem('userEmail'); // Pega o email do usuário armazenado

        // Verifica se o email do usuário existe no sessionStorage
        if (!userEmail) {
            alert('Usuário não selecionado');
            return;
        }
        document.getElementById("user-orders-title").textContent = `Orders from ${userEmail}`;

        const response = await fetch('/Admin/GetUserOrders', {
            method: 'GET',
            headers: {
                'User-Email': userEmail,  // Passa o email do usuário como cabeçalho
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


function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa em 0
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

async function fetchOrderDetails(orderId) {
    try {
        const response = await fetch(`/Admin/GetOrderDetails?numero=${orderId}`);
        const orderDetails = await response.json();  // Parseia a resposta JSON
        return orderDetails;
    } catch (error) {
        return [];
    }
}

// Função para exibir as encomendas na página
async function displayOrders(orders) {
    const ordersContainer = document.querySelector('#orders-container');

    if (ordersContainer) {
        for (const order of orders) {
            const totalPrice = order.trotinetes.reduce((total, t) => {
                if (t.modelo === 'SPEEDY Electric Scooter') {
                    return total + (79.99 * t.quantidade);
                } else if (t.modelo === 'GLIDY Scooter') {
                    return total + (49.99 * t.quantidade);
                }
                return total;
            }, 0);

            // Verificar status do pacote
            const stepNumbers = await Promise.all(order.trotinetes.map(async (t) => {
                const orderDetails = await fetchOrderDetails(order.numero);
                const trotineteDetails = orderDetails.find(d => d.trotineteId === t.idTrotinete);
                return trotineteDetails ? trotineteDetails.passoAtual : '-';
            }));

            const packageStatus = stepNumbers.every(step => step === 0) ? '0' : order.condicao;

            const orderElement = document.createElement('div');
            orderElement.classList.add('order');
            orderElement.innerHTML = `
                <div class="order-header">
                    <div class="package-box">
                        <div class="package">Package #${order.numero}</div>
                    </div>
                    <div class="price-box">
                        <div class="price">Price:<br>${totalPrice.toFixed(2)}€</div>
                    </div>
                    <div class="contents-box">
                        <div class="contents-title">Contents:</div>
                        <div class="contents">
                            <!-- Coluna para SPEEDY -->
                            <div class="contents-left">
                                ${Object.entries(order.trotinetes.reduce((acc, t) => {
                                    if (t.modelo === 'SPEEDY Electric Scooter') {
                                        acc[t.modelo] = (acc[t.modelo] || 0) + t.quantidade;
                                    }
                                    return acc;
                                }, {})).map(([modelo, quantidade]) => `
                                    <div class="speedy">
                                        ${modelo} x${quantidade}
                                    </div>
                                `).join('')}
                            </div>
                            
                            <!-- Coluna para GLIDY e outros -->
                            <div class="contents-right">
                                ${Object.entries(order.trotinetes.reduce((acc, t) => {
                                    if (t.modelo !== 'SPEEDY Electric Scooter') {
                                        acc[t.modelo] = (acc[t.modelo] || 0) + t.quantidade;
                                    }
                                    return acc;
                                }, {})).map(([modelo, quantidade]) => `
                                    <div class="glidy">
                                        ${modelo} x${quantidade}
                                    </div>
                                `).join('')}
                            </div>
                        </div>
                    </div>                       
                    <div class="status-box">
                        <div class="status">Package status:<br>${packageStatus}</div> <!-- Atualizado -->
                    </div>
                    <div class="delivery-box">
                        <div class="delivery">Delivery date:<br>${formatDate(order.dataEntrega)}</div>
                    </div>
                </div>

                <div class="order-details">
                    <table>
                        <thead>
                            <tr>
                                <th>Model</th>
                                <th>Step Number</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${await Promise.all(order.trotinetes.map(async (t) => {
                                const orderDetails = await fetchOrderDetails(order.numero);
                                const trotineteDetails = orderDetails.find(d => d.trotineteId === t.idTrotinete);
                                const passoAtual = trotineteDetails ? trotineteDetails.passoAtual : '-';
                                if (passoAtual == 0){
                                    return `
                                        <tr>
                                            <td onclick="goToPage('${t.modelo}', ${1})">${t.modelo}</td>
                                            <td>-</td>
                                        </tr>
                                    `;
                                }
                                else{
                                    return `
                                        <tr>
                                            <td onclick="goToPage('${t.modelo}', ${passoAtual})">${t.modelo}</td>
                                            <td>${passoAtual}</td>
                                        </tr>
                                    `;
                                }
                            })).then(results => results.join(''))}
                        </tbody>
                    </table>
                </div>
            `;
            ordersContainer.appendChild(orderElement);
        }
    }
}



// Chamar a função para carregar as encomendas assim que a página for carregada
document.addEventListener('DOMContentLoaded', loadOrders);


// Função para voltar à página principal
document.querySelector(".back-button").addEventListener("click", () => {
    window.location.href = "/Admin/UsersList";
});