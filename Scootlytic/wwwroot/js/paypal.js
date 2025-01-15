const payment = document.querySelector('.pay-button');
const emailInput = document.querySelector('input[type="email"]');

if (payment) {
    payment.addEventListener('click', async (event) => {
        if (!emailInput.value) {
            event.preventDefault();
            alert("Por favor, insira o seu e-mail antes de efetuar o pagamento.");
        } else {
            try {
                const response = await fetch('/Cart/FinalizarCompra', {
                    method: "POST",
                    headers: {
                        'User-Email': sessionStorage.getItem("userEmail"),
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ MetodoPagamento: "Paypal" })
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
