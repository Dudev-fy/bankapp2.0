function setCookie(name, value, minutes) { 
    const expires = new Date();
    expires.setTime(expires.getTime() + minutes * 60 * 1000);
    document.cookie = `${name}=${value};expires=${expires.toUTCString()};path=/`;
}

const getToken = async (event) => {
    event.preventDefault();

    const account = document.getElementById('account').value;

    try {
        const response = await fetch('http://localhost:3000/api/token', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({account: account})
        });

        if (response.ok) {
            const token = await response.text();
            setCookie('usertoken', token, 5);
        }
        else {
            alert('failed to get token');
        }
    }
    catch (error) {
        console.log('error:', error);
    }
};

const HandleLogin = async (event) => {
    event.preventDefault();

    const account = document.getElementById('account').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('http://localhost:5000/api/control/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ account: account, password: password }),
        });

        if (response.ok) {
            getToken(event);
            window.location.href = 'dashboard.html';
        } else if (response.status == 401) {
            alert('invalid account or password');
        } else {
            alert('an error ocurred during the authentication');
        }
    } catch (error) {
        console.error('error', error);
    }
};

document.getElementById('loginForm').addEventListener('submit', HandleLogin);
