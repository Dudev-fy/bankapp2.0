using Microsoft.AspNetCore.Mvc;
using Bank;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace BankApi.Controllers;

[ApiController]
[Route("api/control")]
[EnableCors]
public class AuthControl : ControllerBase
{
    private readonly ILoginService _checkUser;
    public readonly IBalance _getBalance;
    public readonly IName _getName;
    public readonly IName _checkAccount;
    public readonly ITransfer _getTransfer;
    public readonly ISignIn _signUser;
    public readonly IInsertStatement _inStatement;
    public readonly IRetriveFK _getFK;
    public readonly IRetriveStatement _getStatement;
    public readonly IFetchStatements _getAllStatements;

    public AuthControl(ILoginService checkUser, IBalance getBalance, IName getName, IName checkAccount, ITransfer getTransfer, ISignIn signUser, IInsertStatement inStatement, IRetriveFK getFK, IRetriveStatement getStatement, IFetchStatements getAllStatements)
    {
        _checkUser = checkUser;
        _getBalance = getBalance;
        _getName = getName;
        _checkAccount = checkAccount;
        _getTransfer = getTransfer;
        _signUser = signUser;
        _inStatement = inStatement;
        _getFK = getFK;
        _getStatement = getStatement;
        _getAllStatements = getAllStatements;
    }

    public class LoginModel
    {
        public string Account {get; set;}
        public string Password {get; set;}
    }

    public class NameModel
    {
        public string Account {get; set;}
    }

    public class TransferModel
    {
        public string Source {get; set;}
        public string Destiny {get; set;}
        public double Amount {get; set;}
    }

    public class BalanceModel
    {
        public string Account {get; set;}
    }

    public class SignInModel
    {
        public string Name {get; set;}
        public string Account {get; set;}
        public string Password {get; set;}
        public double Balance {get; set;}

    }

    public class InsertStatementModel
    {
        public char Operation {get; set;}
        public double Value {get; set;}
        public string Source {get; set;}
        public string Destiny {get; set;}
    }

    public class RetriveStatementModel
    {
        public int IdStatement {get; set;}
    }

    public class GetAllStatementsModel
    {
        public string account {get; set;}
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        bool success = _checkUser.CheckUser(model.Account, model.Password);
        if (success)
        {
            return Ok();
        }
        return Unauthorized();
    
    }

    // modify 'signin' logic to use try catch and transaction rollback commit

    [HttpPost("signin")]
    public IActionResult SignIn([FromBody] SignInModel model)
    {   
        bool isSigned = _signUser.signUser(model.Name, model.Account, model.Password, model.Balance);
        if (isSigned)
        {
            return Ok();
        }
        return Unauthorized();
    }

    [HttpPost("balance")]
    public IActionResult Balance([FromBody] BalanceModel model)
    {   
        double balance = _getBalance.getBalance(model.Account);
        return Ok(balance);
    }

    [HttpPost("name")]
    public IActionResult Name([FromBody] NameModel model)
    {   
        if (model.Account == "")
        {
            return NoContent();
        }
        
        bool isName = _checkAccount.checkAccount(model.Account);
        if (isName)
        {
            string userName = _getName.getName(model.Account);
            return Ok(userName);
        }
        return NotFound();
    }

    // modify 'transfer' logic to use try catch and transaction rollback commit

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferModel model)
    {   
        double sourceBalance = _getBalance.getBalance(model.Source);
        if (sourceBalance >= model.Amount)
        {
            bool isTransfered = _getTransfer.getTransfer(model.Source, model.Destiny, model.Amount);
            if (isTransfered)
            {
                return Ok();
            }
            return Unauthorized();
        } 
        return Unauthorized();
    }

    [HttpPost("instatement")]
    public IActionResult InStatement([FromBody] InsertStatementModel model)
    {
        int sourceFK = _getFK.getFK(model.Source);
        int destinyFK = _getFK.getFK(model.Destiny);

        if (sourceFK > 0 || destinyFK > 0)
        {
            int statementPK = _inStatement.inStatement(model.Operation, model.Value, sourceFK, model.Source, destinyFK, model.Destiny);

            if (statementPK > 0)
            {
                return Ok(statementPK);
            }
            return Unauthorized();
        }
        return NoContent();
    }

    [HttpPost("getstatement")]
    public IActionResult RetriveStatement([FromBody] RetriveStatementModel model)
    {
        var data = _getStatement.getStatement(model.IdStatement);
        if (data == null)
        {
            return Unauthorized();
        }
        return Ok(data);
    }

    [HttpPost("fetchstatement")]
    public IActionResult GetAllStatements([FromBody] GetAllStatementsModel model)
    {
        var query = _getAllStatements.getAllStatements(model.account);
        return Ok(query.ToList());
    }
}