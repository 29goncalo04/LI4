function getQueryParams() {
    const params = new URLSearchParams(window.location.search);
    return {
        name: params.get('name'),
        price: params.get('price'),
        color: params.get('color'),
        reference: params.get('reference'),
    };
}

document.addEventListener('DOMContentLoaded', () => {
    const { name, price, color, reference } = getQueryParams();
    if (name) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(3) td:nth-of-type(2)').textContent = name;
    }
    if (name) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(2) td:nth-of-type(2)').textContent = name.includes('GLIDY') ? 'GLIDY' : 'SPEEDY';
    }
    if (reference) {
        document.querySelector('.section:nth-of-type(1) table tr:nth-of-type(1) td:nth-of-type(2)').textContent = reference;
    }
    if (color) {
        document.querySelector('.section:nth-of-type(2) table tr:nth-of-type(1) td:nth-of-type(2)').textContent = color;
    }
    if (price) {
        const priceElement = document.querySelector('.product-price');
        if (priceElement) priceElement.textContent = price;
    }
});

const backButton = document.querySelector('.close-button');
if (backButton) {
    backButton.addEventListener('click', () => {
        window.location.href = '/Main/Main';
    });
}

const addButton = document.querySelector('.add-to-cart');
if (addButton) {
    addButton.addEventListener('click', () => {
        const { name, color, reference, price} = getQueryParams();
        const formattedPrice = parseFloat(price.replace(',', '.').replace('€', ''));

        if (!name || !color || !reference) {
            alert('Erro: Parâmetros inválidos.');
            return;
        }

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