const jwt = require('jsonwebtoken');
const express = require('express');
const app = express();
const cors = require('cors');
const port = 3000;

app.use(express.json());
app.use(cors());

app.post('/api/token', (req, res) => {
    const payload = {
        account: req.body.account
    };
    const secretKey = 'starwars';
    const token = jwt.sign(payload, secretKey);
    res.send(token);
});

app.get('/api/validate', (req, res) => {
    const extractedToken = req.headers.authorization;
    const secretKey = 'starwars';
    jwt.verify(extractedToken, secretKey, (err) => {
        if (err)  {
            res.sendStatus(401);
            console.log('yes');
        } else {
            res.sendStatus(200);
        }
    });
});

app.listen(port, () => {
    console.log(`API server is running on port ${port}`);
});
