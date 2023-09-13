import React, { useState, useEffect } from 'react';

function RenderBalance() {
  const [balance, setBalance] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

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

  useEffect(() => {
    const fetchData = async () => {

      const token = getCookie('usertoken');
      const [header, payload, signature] = token.split('.');
      const decodedPayload = atob(payload);
      const payloadObject = JSON.parse(decodedPayload);
      const source = payloadObject.account;

      try {
        const response = await fetch('http://localhost:5000/api/control/balance', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({account: source}),
        });

        if (response.ok) {
          const data = await response.json();
          setBalance(data);
        } else {
          console.log('Error:', response.status);
        }
        setIsLoading(false);
      } catch (error) {
        console.error('Error fetching data', error);
      }
    };

    fetchData();
  },);

  return (
    <div>
      {isLoading ? (
        <div className="matte-box-overlay"></div>
      ) : (
        <div>
          <h2 className="text-center mb-4 bottom" style={{ bottom: '-25px' }}>
            Saldo disponível: {balance},00 R$
          </h2>
        </div>
      )}
    </div>
  );
}

export default RenderBalance;
