document.addEventListener("DOMContentLoaded", function () {

    const userEmail = sessionStorage.getItem("userEmail");
    const trashIcons = document.querySelectorAll(".trash-icon");
    const backArrow = document.querySelector(".back-arrow");
    const paypal = document.querySelector(".paypal");
    const mbway = document.querySelector(".mbway");
    const totalPriceElement = document.querySelector(".totalprice");

    if (userEmail) {
        fetch('/Cart/GetCartItems', {
            method: 'GET',  // Requisição GET
            headers: {
                'Content-Type': 'application/json',
                'User-Email': userEmail  // Envia o email no cabeçalho
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                renderCartItems(data.items);  // Preenche a página com os itens
            }
        })
        .catch(error => {
            console.error("Erro de requisição AJAX:", error);
        });
    } else {
        console.error("Email não encontrado no sessionStorage");
    }

    if (backArrow) {
        backArrow.addEventListener("click", () => {
            window.location.href = "/Main/Main";
        });
    }

    if (paypal) {
        paypal.addEventListener("click", () => {
            window.location.href = "/Cart/Paypal";
        });
    }

    if (mbway) {
        mbway.addEventListener("click", () => {
            window.location.href = "/Cart/Mbway";
        });
    }
});


// Função para renderizar os itens do carrinho na página
function renderCartItems(items) {
    const cartContainer = document.getElementById("cart-items-container");
    let total = 0;
    const itemMap = {}; // Mapeamento para agrupar as trotinetes e somar as quantidades

    // Agrupar os itens por modelo
    items.forEach(item => {
        const modelo = item.trotinete.modelo;
        let preco = 0;

        // Definir o preço de acordo com o modelo
        if (modelo === "SPEEDY Electric Scooter") {
            preco = 79.99;
        } else if (modelo === "GLIDY Scooter") {
            preco = 49.99;
        }

        // Verificar se o modelo já existe no itemMap, caso contrário inicializar
        if (!itemMap[modelo]) {
            itemMap[modelo] = {
                modelo: modelo,
                quantidade: 0,
                preco: preco,
                imagePath: modelo === "SPEEDY Electric Scooter" ? "/images/SpeedyScooter.png" : "/images/GlidyScooter.png"
            };
        }

        // Incrementar a quantidade do modelo
        itemMap[modelo].quantidade++;
    });

    // Criar e exibir os produtos no carrinho
    for (const modelo in itemMap) {
        const product = itemMap[modelo];
        const totalPorProduto = product.preco * product.quantidade; // Calcular o preço total para cada modelo

        const productDiv = document.createElement("div");
        productDiv.classList.add("product");
        productDiv.dataset.id = modelo; // Aqui você pode usar o modelo ou id

        // Adicionar conteúdo ao div do produto
        productDiv.innerHTML = `
            <img class="scooter" src="${product.imagePath}" alt="${product.modelo}">
            <div class="details">
                <div class="quantity-wrapper">
                    <p class="quantity-label">Quantity:</p>
                    <p class="quantity-value">x${product.quantidade}</p> <!-- Quantidade total do modelo -->
                </div>
            </div>
            <div class="price-wrapper">
                <p class="price-label">Price:</p>
                <p class="price-value">${totalPorProduto}€</p> <!-- Preço total por modelo -->
            </div>
            <img class="trash-icon" src="/images/lixo.png" alt="Remover">
        `;

        // Adicionar o produto ao container
        cartContainer.appendChild(productDiv);

        // Atualizar o total
        total += totalPorProduto;
    }

    // Exibir o total
    const totalDiv = document.getElementById("total-price");
    if (totalDiv) {
        totalDiv.textContent = `TOTAL: ${total}€`;
    }


    const trashIcons = document.querySelectorAll(".trash-icon");
    trashIcons.forEach(icon => {
        icon.addEventListener("click", () => {
            const productElement = icon.closest(".product");
            const trotineteId = productElement.getAttribute("data-id");
    
            if (productElement && trotineteId) {
                // Remove visualmente o elemento da página
                productElement.remove();
    
                // Atualiza o servidor usando GET, já que a API agora usa GET
                fetch(`/Cart/RemoveFromCart?modelo=${trotineteId}`, {
                    method: "GET", // Alterado para GET
                    headers: {
                        'User-Email': sessionStorage.getItem("userEmail")
                    }
                })
                .then(response => response.json())
                .then(data => {
                    // Verifica se a resposta foi bem-sucedida
                    if (data.newTotalPrice !== undefined) {
                        // Atualiza o preço total
                        const totalPriceElement = document.getElementById("total-price");
                        totalPriceElement.textContent = data.newTotalPrice + "€";
                    } else {
                        console.error("Erro ao remover item:", data.error);
                    }
                })
                .catch(error => console.error("Erro ao remover item:", error));
            } else {
                console.log("Elemento de produto ou ID não encontrado.");
            }
        });
    });
}