function trackText(inputId, remainderId, minLength = 5, maxLength = 100) {
    const input = document.getElementById(inputId);
    const remainder = document.getElementById(remainderId);
    updateRemainder(input, remainder, minLength, maxLength);

    input.addEventListener('input', () => updateRemainder(input, remainder, minLength, maxLength));
}

function updateRemainder(input, remainder, minLength, maxLength) {
    const characters = input.value.length;

    if (characters < minLength || characters >= maxLength) {
        remainder.classList.add('text-danger');
    } else {
        remainder.classList.remove('text-danger');
    }

    remainder.innerText = characters;
}
