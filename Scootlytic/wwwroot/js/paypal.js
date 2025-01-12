const payment = document.querySelector('.pay-button');
const emailInput = document.querySelector('input[type="email"]'); // Selecione o campo de e-mail

if (payment) {
    payment.addEventListener('click', (event) => {
        // Verifica se o e-mail foi inserido
        if (!emailInput.value) {
            event.preventDefault(); // Impede o envio do formulário ou redirecionamento
            alert("Por favor, insira o seu e-mail antes de efetuar o pagamento.");
        } else {
            // Caso o e-mail tenha sido inserido, permite o pagamento
            alert("Pagamento efetuado com sucesso.");
            window.location.href = '../../pages/main.html';
        }
    });
}