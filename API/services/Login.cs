using System;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Bank
{   
    public interface ILoginService
    {
        bool CheckUser(string a, string b);
    }

    class Login : ILoginService
    {
        private readonly ApplicationDbContext _dbContext;

        public Login(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CheckUser(string a, string b)
        {   
            var data = _dbContext.Accounts
                .Join(
                    _dbContext.Salts,
                    account => account.IDACCOUNT,
                    salt => salt.ID_ACCOUNT,
                    (account, salt) => new
                    {   
                        ACCOUNT_NUMBER = account.ACCOUNT_NUMBER,
                        PASSWORD = account.PASSWORD,
                        SALT = salt.SALT
                    })
                .Where(account => account.ACCOUNT_NUMBER == a)
                .FirstOrDefault();
            
            if (data.PASSWORD != null)
            {
                string password = data.PASSWORD;
                string salt = data.SALT;

                string concatenaded = string.Concat(b, salt);
                byte[] concatenadedByte = Encoding.UTF8.GetBytes(concatenaded);

                SHA256 sha256 = SHA256.Create();
                byte[] hashedBytes = sha256.ComputeHash(concatenadedByte);

                string hashedString = BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToLower();

                if (password == hashedString)
                {
                    Console.WriteLine("Access granted");
                    return true;
                }
                else
                {
                    Console.WriteLine("Access denied");
                    return false;
                } 
            }
            return false;
        } 
    } 
}