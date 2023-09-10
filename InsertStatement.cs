using System;
using MySql.Data.MySqlClient;

namespace Bank
{
    public interface IInsertStatement
    {
        int inStatement(char operation, double value, int fkSource, string source, int fkDestiny, string destiny);     
    }

    class InsertStatement : IInsertStatement
    {
        private MySqlConnection connection;
        private int statementPK = 0;

        public InsertStatement()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public int inStatement(char operation, double value, int fkSource, string source, int fkDestiny, string destiny)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO STATEMENT VALUES(NULL, @OPERATION, @VALUE, @FKSOURCE, @SOURCE, @FKDESTINY, @DESTINY, DEFAULT); SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OPERATION", operation);
                    command.Parameters.AddWithValue("@VALUE", value);
                    command.Parameters.AddWithValue("@FKSOURCE", fkSource);
                    command.Parameters.AddWithValue("@SOURCE", source);
                    command.Parameters.AddWithValue("@FKDESTINY", fkDestiny);
                    command.Parameters.AddWithValue("@DESTINY", destiny);
                    statementPK = Convert.ToInt32(command.ExecuteScalar());
                    return statementPK;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query Error" + ex.Message);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}