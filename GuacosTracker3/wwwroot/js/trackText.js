function trackText(inputId, remainderId, minLength = 5, maxLength = 100) {
    const input = document.getElementById(inputId);
    const remainder = document.getElementById(remainderId);

    input.addEventListener('input', () => {
        const characters = input.value.length;

        if (characters < minLength || characters >= maxLength) {
            remainder.classList.add('text-danger');
        } else {
            remainder.classList.remove('text-danger');
        }

        remainder.innerText = characters;
    })
}
