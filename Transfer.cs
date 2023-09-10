using System;
using MySql.Data.MySqlClient;

namespace Bank
{
    public interface ITransfer
    {
        bool getTransfer(string source, string destiny, double amount);
    }

    class Transfer : ITransfer
    {   
        private MySqlConnection connection;

        public Transfer()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public bool getTransfer(string source, string destiny, double amount)
        {
            try
            {   
                connection.Open();
                string query = "SELECT BALANCE FROM ACCOUNT WHERE ACCOUNT_NUMBER = @NUMBER";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NUMBER", source);
                    double sourceBalance = Convert.ToDouble(command.ExecuteScalar());
                    double sourceNewBalance = sourceBalance - amount;
                    string query1 = "UPDATE ACCOUNT SET BALANCE = @NEWBALANCE WHERE ACCOUNT_NUMBER = @SOURCE";
                    using (MySqlCommand command1 = new MySqlCommand(query1, connection))
                    {
                        command1.Parameters.AddWithValue("@NEWBALANCE", sourceNewBalance);
                        command1.Parameters.AddWithValue("@SOURCE", source);
                        command1.ExecuteNonQuery();
                    }
                }
                string query2 = "SELECT BALANCE FROM ACCOUNT WHERE ACCOUNT_NUMBER = @NUMBER";
                using (MySqlCommand command = new MySqlCommand(query2, connection))
                {
                    command.Parameters.AddWithValue("@NUMBER", destiny);
                    double destinyBalance = Convert.ToDouble(command.ExecuteScalar());
                    double destinyNewBalance = destinyBalance + amount;
                    string query3 = "UPDATE ACCOUNT SET BALANCE = @NEWBALANCE WHERE ACCOUNT_NUMBER = @DESTINY";
                    using (MySqlCommand command1 = new MySqlCommand(query3, connection))
                    {
                        command1.Parameters.AddWithValue("@NEWBALANCE", destinyNewBalance);
                        command1.Parameters.AddWithValue("@DESTINY", destiny);
                        command1.ExecuteNonQuery();
                        return true;
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
    }
}