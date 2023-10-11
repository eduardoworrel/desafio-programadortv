const form = document.getElementById('form');
const result = document.getElementById('result');

form.onsubmit = function(event){
    event.preventDefault();

    const formData = new FormData(form);

    fetch('/', {
        method: 'POST',
        body: formData
    })
    .then((response) => response.json())
    .then((data) => {
        for(let key in data){
            const p = document.createElement('p');
            p.innerHTML = `${key}: ${data[key]}`;
            result.appendChild(p);
        }
    })
}
