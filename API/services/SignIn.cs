using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Bank
{
    public interface ISignIn
    {
        bool signUser(string name, string account, string password, double balance); 
    }
    
    class SignIn : ISignIn
    {
        private MySqlConnection connection;
        private static Random random = new Random();
        private int userFK = 0;

        public SignIn()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public bool signUser(string name, string account, string password, double balance)
        {
            try
            {
                // use 'executereader' in the other codes for better coding and fewer database use
                // also use 1 query to multiple inserts when possible

                connection.Open();
                string query = "SELECT COUNT(*) FROM ACCOUNT WHERE ACCOUNT_NUMBER = @ACCOUNT";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ACCOUNT", account);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 0)
                    {   
                        HashSet<int> generatedNumbers = new HashSet<int>();
                        int number;
                        do
                        {
                            number = random.Next(100, 1000);
                        }
                        while (!generatedNumbers.Add(number)); 

                        string query1 = "INSERT INTO ACCOUNT VALUES(NULL, @ACCOUNT, SHA2(CONCAT(@PASSWORD, @SALT), 256), @BALANCE); SELECT LAST_INSERT_ID();";
                        using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                        {
                            command1.Parameters.AddWithValue("@ACCOUNT", account);
                            command1.Parameters.AddWithValue("@PASSWORD", password);
                            command1.Parameters.AddWithValue("@SALT", number);
                            command1.Parameters.AddWithValue("@BALANCE", balance);
                            userFK = Convert.ToInt32(command1.ExecuteScalar());

                            if (userFK > 0)
                            {
                                string query3 = "INSERT INTO SALT VALUES(NULL, @NUMBER, @FK); INSERT INTO USER VALUES(NULL, @NAME, @FK);";
                                using (MySqlCommand command3 = new MySqlCommand(query3, connection))
                                {
                                    command3.Parameters.AddWithValue("@NUMBER", number);
                                    command3.Parameters.AddWithValue("@FK", userFK);
                                    command3.Parameters.AddWithValue("@NAME", name);
                                    command3.ExecuteNonQuery();
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                    return false;
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
    }
}