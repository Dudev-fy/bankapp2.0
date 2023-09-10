import React, {useEffect, useState} from 'react';

const FetchStatements = () => {
  const [statements, setStatement] = useState([]);
  const [account, setAccount] = useState('');

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
      setAccount(source);

      try {
        const response = await fetch('http://localhost:5000/api/control/fetchstatement', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({account: source}),
        });

        if (response.ok) {
          const data = await response.json();
          const newData = data.map(s => {
            const splitedString = s.datahora.split('T');
            const splitedString1 = splitedString[0].split('-');
            const formattedString = `${splitedString1[2]}/${splitedString1[1]}/${splitedString1[0]} ás ${splitedString[1]}`;
            const newValue = s.value + ',00';
            return { ...s, datahora: formattedString, value: newValue };
          });
          setStatement(newData);
        }
        else {
          console.log('access denied');
        }

      } catch (error) {
          console.log('error fetching data', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className='container'>
      <div className='row justify-content-center'>
        <div className='col-lg-12'>
          <div className='datatypes'>
            <div className='data'>
              <h4>Data</h4>
            </div>
            <div className='account'>
              <h4>Conta</h4>
            </div>
            <div className='name'>
              <h4>Nome</h4>
            </div>
            <div className='value'>
              <h4>Valor(R$)</h4>
            </div>
          </div>
          {statements.map((statement, index) => (
            <div key={index} className='statement'>
              <div className='property'>
                <p className='text'>{statement.datahora}</p>
              </div>
              <div className='property'>
                <p className='text'>
                  {statement.source === account ? `${statement.destiny}` : `${statement.source}`}
                </p>
              </div>
              <div className='nameproperty'>
                <p className='text'>
                  {statement.source === account ? `${statement.userDestinyName}` : `${statement.userSourceName}`}
                </p>
              </div>
              <div className='lastproperty'>
                <p className='text'style={statement.source === account ? { color: 'red' }: {color: 'green'}}>
                  {statement.source === account ? `-${statement.value}` : `+${statement.value}`}
                </p>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}

export default FetchStatements;