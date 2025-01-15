const payment = document.querySelector('.pay-button');
const phoneInput = document.querySelector('input[type="email"]'); // Substitua por uma classe ou ID apropriada se o campo não for de e-mail

if (payment) {
    payment.addEventListener('click', async (event) => {
        // Verifica se o número de telemóvel foi inserido
        if (!phoneInput.value) {
            event.preventDefault(); // Impede o envio do formulário ou redirecionamento
            alert("Por favor, insira o seu número de telemóvel antes de efetuar o pagamento.");
        } else if (!/^(9\d{8})$/.test(phoneInput.value)) {
            // Verifica se o número tem 9 dígitos e começa por 9
            event.preventDefault(); 
            alert("Por favor, insira um número de telemóvel válido (9 dígitos e começando por 9).");
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
