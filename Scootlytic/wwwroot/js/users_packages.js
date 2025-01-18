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

function formatDate(dateString) {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Mês começa em 0
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

// Função para exibir as encomendas na página

//const response = await fetch(`/Admin/GetOrderDetails?numero=${orderId}`);

// Função para buscar os detalhes da ordem
async function fetchOrderDetails(orderId) {
    try {
        const response = await fetch(`/Admin/GetOrderDetails?numero=${orderId}`);
        const orderDetails = await response.json();  // Parseia a resposta JSON
        console.log("Dados recebidos do backend:", orderDetails);  // Adicionando log
        return orderDetails;
    } catch (error) {
        console.error("Erro ao buscar os detalhes da ordem:", error);
        return [];
    }
}

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
                            ${order.trotinetes.filter(t => t.modelo === 'SPEEDY Electric Scooter').map(t => `
                                <div class="speedy">
                                    ${t.modelo} x${t.quantidade}
                                </div>
                            `).join('')}
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
                                console.log("Trotinete:", t);
                                const orderDetails = await fetchOrderDetails(order.numero);
                                
                                console.log("Detalhes da ordem:", orderDetails);  // Adicionando log

                                const trotineteDetails = orderDetails.find(d => d.trotineteId === t.idTrotinete);
                                console.log(t.idTrotinete);
                                
                                if (!trotineteDetails) {
                                    console.error(`Detalhes da trotinete não encontrados para o ID ${t.idTrotinete}`);
                                }

                                const passoAtual = trotineteDetails ? trotineteDetails.passoAtual : '-';
                                //console.log(`PassoAtual para a trotinete ${t.idTrotinete}: ${passoAtual}`);
                                if (passoAtual == 0){
                                    return `
                                        <tr>
                                            <td onclick="goToPage('${t.modelo}')">${t.modelo}</td>
                                            <td>-</td>
                                        </tr>
                                    `;
                                }
                                else{
                                    return `
                                        <tr>
                                            <td onclick="goToPage('${t.modelo}')">${t.modelo}</td>
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






function goToPage(model) {
    // Função para navegar para a página detalhada do modelo de trotinete
    if(model === "SPEEDY Electric Scooter") window.location.href = "/Scooter/stepsSpeedy";  
    else if (model === "GLIDY Scooter") window.location.href = "/Scooter/stepsGlidy";
    else alert("page not found");
    //window.location.href = "/Scooter/stepsSpeedy";
}








// Chamar a função para carregar as encomendas assim que a página for carregada
document.addEventListener('DOMContentLoaded', loadOrders);


// Função para voltar à página principal
document.querySelector(".back-button").addEventListener("click", () => {
    window.location.href = "/Main/Main";
});