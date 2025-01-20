document.addEventListener("DOMContentLoaded", function () {

    const userEmail = sessionStorage.getItem("userEmail");
    const backArrow = document.querySelector(".back-arrow");
    const paypal = document.querySelector(".paypal");
    const mbway = document.querySelector(".mbway");

    if (userEmail) {
        fetch('/Cart/GetCartItems', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'User-Email': userEmail
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                renderCartItems(data.items);
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


function renderCartItems(items) {
    const cartContainer = document.getElementById("cart-items-container");
    let total = 0;
    const itemMap = {};

    items.forEach(item => {
        const modelo = item.trotinete.modelo;
        let preco = 0;

        if (modelo === "SPEEDY Electric Scooter") {
            preco = 79.99;
        } else if (modelo === "GLIDY Scooter") {
            preco = 49.99;
        }
        if (!itemMap[modelo]) {
            itemMap[modelo] = {
                modelo: modelo,
                quantidade: 0,
                preco: preco,
                imagePath: modelo === "SPEEDY Electric Scooter" ? "/images/SpeedyScooter.png" : "/images/GlidyScooter.png"
            };
        }
        itemMap[modelo].quantidade++;
    });
    for (const modelo in itemMap) {
        const product = itemMap[modelo];
        const totalPorProduto = product.preco * product.quantidade;

        const productDiv = document.createElement("div");
        productDiv.classList.add("product");
        productDiv.dataset.id = modelo;

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
        cartContainer.appendChild(productDiv);
        total += totalPorProduto;
    }
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
                productElement.remove();
                    fetch(`/Cart/RemoveFromCart?modelo=${trotineteId}`, {
                    method: "GET",
                    headers: {
                        'User-Email': sessionStorage.getItem("userEmail")
                    }
                })
                .then(response => response.json())
                .then(data => {
                    if (data.newTotalPrice !== undefined) {
                        const totalPriceElement = document.getElementById("total-price");
                        totalPriceElement.textContent = parseFloat(data.newTotalPrice).toFixed(2) + "€";
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