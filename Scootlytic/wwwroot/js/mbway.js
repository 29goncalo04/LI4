const payment = document.querySelector('.pay-button');
const phoneInput = document.querySelector('input[type="email"]'); // Substitua por uma classe ou ID apropriada se o campo não for de e-mail

if (payment) {
    payment.addEventListener('click', (event) => {
        // Verifica se o número de telemóvel foi inserido
        if (!phoneInput.value) {
            event.preventDefault(); // Impede o envio do formulário ou redirecionamento
            alert("Por favor, insira o seu número de telemóvel antes de efetuar o pagamento.");
        } else if (!/^(9\d{8})$/.test(phoneInput.value)) {
            // Verifica se o número tem 9 dígitos e começa por 9
            event.preventDefault(); 
            alert("Por favor, insira um número de telemóvel válido (9 dígitos e começando por 9).");
        } else {
            // Número de telemóvel válido
            alert("Pagamento efetuado com sucesso.");
            window.location.href = '/Main/Main';
        }
    });
}
