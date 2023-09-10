using System;
using System.Linq;

namespace Bank
{
    public interface IFetchStatements
    {
        IQueryable<object> getAllStatements(string account);
    }

    class FetchStatements : IFetchStatements
    {
        private readonly ApplicationDbContext _dbContext;

        public FetchStatements(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<object> getAllStatements(string account)
        {
            var query = from s in _dbContext.Statements
                        join su in _dbContext.Users on s.ID_SOURCE equals su.ID_ACCOUNT
                        join du in _dbContext.Users on s.ID_DESTINY equals du.ID_ACCOUNT
                        where s.SOURCE == account || s.DESTINY == account
                        select new
                        {
                            OPERATION = s.OPERATION,
                            SOURCE = s.SOURCE,
                            UserSourceName = su.NAME,
                            DESTINY = s.DESTINY,
                            UserDestinyName = du.NAME,
                            VALUE = s.VALUE,
                            DATAHORA = s.DATA_HORA
                        };
            return query;
        }
    }
}