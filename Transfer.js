import React, { useState } from 'react';
import RenderBalance from './Balance';

const FormWithTransition = () => {
  const [name, setName] = useState('');
  const [accountnumber, setAccountNumber] = useState('');
  const [notfound, setNotFound] = useState(false);
  const [value, setValue] = useState('');
  const [isnull, setNull] = useState(false);
  const [valueIsNull, setValueIsNull] = useState(false);
  const [noFunds, setNoFunds] = useState(false);
  const [isTransfered, setTransfered] = useState(false);
  const [statement, setStatement] = useState({ 
    value: '',
    source: '',
    sourceName: '',
    destiny: '',
    destinyName: '',
    dataHora: '',
    isStatement: false 
  });

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

  const Transfer = async (event) => {
    event.preventDefault();
    const account = accountnumber;
    const amount = value;
    setValueIsNull(false);
    setNoFunds(false);

    const token = getCookie('usertoken');
    const [header, payload, signature] = token.split('.');
    const decodedPayload = atob(payload);
    const payloadObject = JSON.parse(decodedPayload);
    const source = payloadObject.account;

    try {
      const response = await fetch('http://localhost:5000/api/control/balance', {
        method: 'POST',
        headers: {
          'Content-Type' : 'application/json',
        },
        body: JSON.stringify({account: source}),
      });

      if (response.ok)
      {
        const balance = await response.json();
        if (amount > balance) {
          setNoFunds(true);

        } else if (amount === '') {
          setValueIsNull(true);
        } else {
          try {
            const response = await fetch('http://localhost:5000/api/control/transfer', {
              method: 'POST',
              headers: {
                'Content-Type' : 'application/json',
              },
              body: JSON.stringify({source : source, destiny : account, amount : amount}),
            });
      
            if (response.ok) {
              try {
                const response1 = await fetch('http://localhost:5000/api/control/instatement', {
                  method: 'POST',
                  headers: {
                    'Content-Type' : 'application/json',
                  },
                  body: JSON.stringify({operation : 'T', value : amount, source : source, destiny : account}),
                });
      
                if (response1.ok) {
                  const statementID = await response1.text();
                  setTransfered(true);
                  try {
                    const response2 = await fetch('http://localhost:5000/api/control/getstatement', {
                      method: 'POST',
                      headers: {
                        'Content-Type' : 'application/json',
                      },
                      body: JSON.stringify({idstatement : statementID}),
                    });
      
                    if (response2.ok) {
                      const statementData = await response2.json();
                      const keys = Object.keys(statementData);
      
                      setStatement(prevState => ({
                        ...prevState,
                        value: statementData[keys[4]],
                        source: statementData[keys[0]],
                        sourceName: statementData[keys[1]],
                        destiny: statementData[keys[2]],
                        destinyName: statementData[keys[3]],
                        dataHora: statementData[keys[5]],
                        isStatement: true
                      }));
      
                      const con = document.getElementById('con');
                      con.classList.add('statement');
                      const col = document.getElementById('col');
                      col.classList.add('statement');
                      
                      setTimeout(() => {
                        setTransfered(false);
                      }, 3000);
                    }
                  }
                  catch (error) {
                    console.error('error fecthing data', error);
                  }
                }
              }
              catch (error) {
                console.error('error fetching data', error);
              }
            } else {
              console.log("error");
            }
          } catch (error) {
            console.error("error fetching data", error);
          }
        };
        }
      }
    catch (error) {
      console.log('error', error);
    }
  };

  const handleChange = (event) => {
    const input = event.target.value;
    const newInput = input.replace(/[^0-9]/g, '');
    setAccountNumber(newInput);
  };

  const handleChangeValue = (event) => {
    const input = event.target.value;
    const cleanedInput = input.replace(/[^0-9]/g, '');
    setValue(cleanedInput);
  };

  const handleClick = async (event) => {
    event.preventDefault();
    const account = accountnumber;
    setNotFound(false);
    setNull(false);

    if (account === '') {
      setName('');
      setNull(true);
    } else {
      setNull(false);
      try {
        const response = await fetch('http://localhost:5000/api/control/name', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ account }),
        });
  
        if (response.ok) {
          const data = await response.text();
          setName(data);
        } else if (response.status === 404){
          setNotFound(true);
        }
      } catch (error) {
        console.error('error', error);
      }
    }
  };

  return (
    <div>
      {statement.isStatement === false && (
        <RenderBalance />
      )}
      <div className="container" id="con">
        <div className="row justify-content-center">
          <div className="col-lg-6" id="col">
            {statement.isStatement === false && (
              <div id="mainForm">
              <form id="myForm" onSubmit={handleClick}>
                <div className="form-group">
                  <label htmlFor="accountnumber" className="form-label">
                    Digite o número da conta
                  </label>
                  <div className="input-group">
                    <input
                      type="text"
                      className="form-control"
                      id="accountnumber"
                      placeholder="account number"
                      value={accountnumber}
                      onChange={handleChange}
                      autocomplete="off"
                    />
                    <div className="input-group-append">
                      <button type="submit" id="button1" className="btn btn-primary">
                        Search
                      </button>
                    </div>
                  </div>
                </div>
              </form>
            </div>
            )}
            {notfound && (
              <div className="text-center" id="notfound">
                <h2 className="small">Conta não encontrada</h2>
              </div>
            )}
            {name && notfound === false && statement.isStatement === false && (
              <div className="text-center" id="result">
                <div>
                  <h2 className="small">Destinatario:<p className="medium">{name}</p></h2>
                </div>
                <form id="transferForm" onSubmit={Transfer}>
                  <div className="form-group">
                    <label htmlFor="amount" className="none"></label>
                    <div className="input-group">
                      <input
                        type="text"
                        className="input"
                        id="amount"
                        placeholder="valor"
                        value={value}
                        onChange={handleChangeValue}
                        autocomplete="off"
                      />
                      <div className="input-group-append">
                        <button type="submit" id="button2" className="btn btn-primary">
                          Transfer
                        </button>
                      </div>
                    </div>
                  </div>
                </form>
              </div>
            )}
            {name && notfound === false && statement.isStatement === false && valueIsNull && (
              <div id="error">
                <p style={{color: 'red'}}>*Digite um valor</p>
              </div>
            )}
            {name && notfound === false && statement.isStatement === false && noFunds && (
              <div id="error">
                <p style={{color: 'red'}}>*Fundos insuficientes</p>
              </div>
            )}
            {isnull && (
              <div id="error">
                <p>*Digite o número da conta</p>
              </div>  
            )}
            {isTransfered && (
              <div className="loadingTransfer">
                <div className="loadingText">
                  <h1>Finalizando os detalhes da transferência ...</h1>
                </div>
                <div className="loader"></div>
              </div>
            )}
            {statement.isStatement === true && (
              <div className="statement">
                <div className="title">
                  <h1 className="titleh1">Extrato Bancário</h1>
                </div>
                <div className="data">
                  <div className="datacon">
                    <h2 className="datah1">Valor:</h2>
                    <p>R$ {statement.value},00</p>
                  </div>
                  <div className="datacon">
                    <h2 className="datah1">Conta origem:</h2>
                    <p>{statement.source}</p>
                  </div>
                  <div className="datacon">
                    <h2 className="datah1">Nome do recipiente:</h2>
                    <p>{statement.sourceName}</p>
                  </div>
                  <div className="datacon">
                    <h2 className="datah1">Conta destino: </h2>
                    <p>{statement.destiny}</p>
                  </div>
                  <div className="datacon">
                    <h2 className="datah1">Nome do destinatário:</h2>
                    <p>{statement.destinyName}</p>
                  </div>
                  <div className="datacon">
                    <h2 className="datah1">Data da operação:</h2>
                    <p>{statement.dataHora}</p>
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>  
  );
};

export default FormWithTransition;