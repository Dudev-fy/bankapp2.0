using System;
using MySql.Data.MySqlClient;

namespace Bank
{
    public interface IRetriveFK
    {   
        int getFK(string account);
    }

    class RetriveFK : IRetriveFK
    {
        private MySqlConnection connection;

        public RetriveFK()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public int getFK(string account)
        {
            try
            {   
                connection.Open();
                string query = "SELECT IDACCOUNT FROM ACCOUNT WHERE ACCOUNT_NUMBER = @ACCOUNT";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ACCOUNT", account);
                    int fk = Convert.ToInt32(command.ExecuteScalar());
                    return fk;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query Error:" + ex.Message);
                return 0;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}