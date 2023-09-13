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

const SignIn = async (event) => {
    event.preventDefault();
  
    const name = document.getElementById('name').value;
    const account = document.getElementById('account').value;
    const password = document.getElementById('password').value;
    const balance = document.getElementById('balance').value;
  
    if (name !== '' && account !== '' && password !== '' && balance !== '') {
      try {
        const response = await fetch('http://localhost:5000/api/control/signin', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ name, account, password, balance }),
        });
  
        if (response.ok) {
          getToken(event);
          window.location.href = 'dashboard.html';
        } else {
          alert('fail');
        }
      } catch (error) {
        console.log('error', error);
      }
    } else {
      alert('Incomplete form :/');
    }
};

document.getElementById('loginForm').addEventListener('submit', SignIn);
