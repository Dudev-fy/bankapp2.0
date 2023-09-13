using System;
using MySql.Data.MySqlClient;

namespace Bank
{
    public interface IName
    {
        string getName(string account);
        bool checkAccount(string account);
    }

    class Name : IName 
    {
        private MySqlConnection connection;

        public Name()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public bool checkAccount(string account)
        {   
            try
            {
                connection.Open();
                string query1 = "SELECT COUNT(*) FROM ACCOUNT WHERE ACCOUNT_NUMBER = @ACCOUNT";
                using (MySqlCommand command = new MySqlCommand(query1, connection))
                {
                    command.Parameters.AddWithValue("@ACCOUNT", account);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 1)
                    {
                        connection.Close();
                        return true;
                    }
                    else
                    {
                        connection.Close();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query error:" + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public string getName(string account)
        {   
            
            try
            {
                connection.Open();
                string query = "SELECT U.NAME FROM USER U INNER JOIN ACCOUNT A ON A.IDACCOUNT = U.ID_ACCOUNT WHERE A.ACCOUNT_NUMBER = @ACCOUNT";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ACCOUNT", account);
                    string userName = Convert.ToString(command.ExecuteScalar());
                    return userName;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query error:" + ex.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}