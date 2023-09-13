using System;
using MySql.Data.MySqlClient;

namespace Bank
{   
    public interface IRetriveStatement
    {
        DataResult getStatement(int IdStatement);
    }

    public class DataResult
    {
        public string Source {get; set;}
        public string SourceName {get; set;}
        public string Destiny {get; set;}
        public string DestinyName {get; set;}
        public double Value {get; set;}
        public string DataHora {get; set;}
    }

    class RetriveStatement : IRetriveStatement
    {
        private MySqlConnection connection;

        public RetriveStatement()
        {
            string connectionString = "server=localhost;port=3306;database=bank_system;uid=root;password=1234";
            connection = new MySqlConnection(connectionString);
        }

        public DataResult getStatement(int IdStatement)
        {
            try
            {
                connection.Open();
                string query = "SELECT S.SOURCE, U.NAME, S.DESTINY, UU.NAME, S.VALUE, S.DATA_HORA FROM STATEMENT S INNER JOIN USER U ON U.ID_ACCOUNT = S.ID_SOURCE INNER JOIN USER UU ON UU.ID_ACCOUNT = S.ID_DESTINY WHERE IDSTATEMENT = @IDSTATEMENT";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IDSTATEMENT", IdStatement);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        DataResult data = new DataResult
                        {
                            Source = reader.GetString(0),
                            SourceName = reader.GetString(1),
                            Destiny = reader.GetString(2),
                            DestinyName = reader.GetString(3),
                            Value = Convert.ToDouble(reader.GetString(4)),
                            DataHora = reader.GetString(5)
                        };
                        return data;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query error" + ex.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}   

