// Função para obter os parâmetros da URL
function getQueryParams() {
    const params = new URLSearchParams(window.location.search);
    return {
        name: params.get('name'),
        price: params.get('price'),
        color: params.get('color'),
        reference: params.get('reference'),
    };
}

// Atualiza os detalhes da página com base nos parâmetros
document.addEventListener('DOMContentLoaded', () => {
    const { name, price, color, reference } = getQueryParams();

    // Atualiza o modelo e o nome do produto
    if (name) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(3) td:nth-of-type(2)').textContent = name;
    }

    // Atualiza a marca do produto
    if (name) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(2) td:nth-of-type(2)').textContent = name.includes('GLIDY') ? 'GLIDY' : 'SPEEDY';
    }

    // Atualiza a referência do produto
    if (reference) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(1) td:nth-of-type(2)').textContent = reference;
    }

    // Atualiza a cor do produto
    if (color) {
        document.querySelector('.section:nth-of-type(2) table tr:nth-of-type(1) td:nth-of-type(2)').textContent = color;
    }

    // Exibir o preço no modal (opcional)
    if (price) {
        const priceElement = document.querySelector('.product-price'); // Elemento fictício para exibir preço
        if (priceElement) priceElement.textContent = price;
    }
});

// Botão de voltar
const backButton = document.querySelector('.close-button');
if (backButton) {
    backButton.addEventListener('click', () => {
        window.location.href = '/Main/Main';
    });
}

// Botão de adicionar ao carrinho
const addButton = document.querySelector('.add-to-cart');
if (addButton) {
    addButton.addEventListener('click', () => {
        const { name, color, reference, price} = getQueryParams();
        const formattedPrice = parseFloat(price.replace(',', '.').replace('€', ''));

        if (!name || !color || !reference) {
            alert('Erro: Parâmetros inválidos.');
            return;
        }

        // Enviar os dados ao servidor via AJAX
        fetch('/Scooter/AddToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'User-Email': sessionStorage.getItem("userEmail"),
            },
            body: JSON.stringify({ name, color, reference, formattedPrice, price:formattedPrice}),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Erro no servidor: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                alert('Produto adicionado ao carrinho.');
            } else {
                alert('Erro ao adicionar ao carrinho: ' + data.message);
            }
        })
        .catch(error => {
            alert('Erro ao adicionar ao carrinho.');
        });
    });
}