function checkToken() {
    const token = getCookie('usertoken');
    if (!token) {
        return;
    }
    fetch('http://localhost:3000/api/validate', {
        method: 'GET',
        headers: {
            'Authorization': token
        }
    })
    .then(response => {
        if (!response.ok) {
            alert('Sua autenticação expirou. Faça Login novamente');
            window.location.href = 'index.html';
        }
    })
    .catch (error => {
        console.error('error ocurred during verification:', error);
    })
}
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        if (cookie.startsWith(name + '=')) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}
window.addEventListener('load', checkToken);