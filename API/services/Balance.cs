using System;
using MySql.Data.MySqlClient;

namespace Bank
{
    public interface IBalance
    {
        double getBalance(string account);
    }

    class Balance : IBalance
    {
        private readonly ApplicationDbContext _dbContext;

        public Balance(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public double getBalance(string account)
        {   
            var balance = _dbContext.Accounts.FirstOrDefault(a => a.ACCOUNT_NUMBER == account);
            if (balance != null)
            {
                return balance.BALANCE;
            }
            return 0;
        }
    }
}
