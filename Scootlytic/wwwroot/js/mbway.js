const payment = document.querySelector('.pay-button');
const phoneInput = document.querySelector('input[type="email"]');

if (payment) {
    payment.addEventListener('click', async (event) => {
        if (!phoneInput.value) {
            event.preventDefault();
            alert("Por favor, insira o seu número de telemóvel antes de efetuar o pagamento.");
        } else if (!/^(9\d{8})$/.test(phoneInput.value)) {
            event.preventDefault(); 
            alert("Por favor, insira um número de telemóvel válido (com 9 dígitos e que comece por 9).");
        } else {
            try {
                const response = await fetch('/Cart/FinalizarCompra', {
                    method: "POST",
                    headers: {
                        'User-Email': sessionStorage.getItem("userEmail"),
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ MetodoPagamento: "MBWAY" })
                });

                const result = await response.json();
                if (result.success) {
                    alert("Pagamento efetuado com sucesso. A sua encomenda foi criada.");
                    window.location.href = '/Main/Main';
                } else {
                    alert("Erro ao processar o pagamento: " + result.error);
                }
            } catch (error) {
                alert("Erro inesperado. Tente novamente.");
            }
        }
    });
}
