using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataUploader
{
    public class DatabaseManager
    {
        SqlConnection connection;

        public DatabaseManager()
        {
            string connectionString = "Data Source=zhengyl-db.database.windows.net; Initial Catalog=zhengyl-db; User ID=liuzhengyang183; Password=COOL_man183";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void Upsert()
        {

        }

        public void CreateTable(string tableName)
        {
            string queryString =
            "CREATE TABLE " + tableName + " (" +
            "time DateTime NOT NULL,    " +
            "acceleration numeric NOT NULL,    " +
            "PRIMARY KEY(time)); ";

            SqlCommand command = new SqlCommand(queryString, connection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("SQL Command Excuted: {0}, {1}", reader[0], reader[1]));
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }
    }
}

//***** Create Table *****
//************************
//"CREATE TABLE Persons (" +
//"ID int NOT NULL,    " +
//"LastName varchar(255) NOT NULL,    " +
//"FirstName varchar(255),    " +
//"Age int,    " +
//"PRIMARY KEY(ID)); ";

//***** Insert Data *****
//"INSERT INTO Persons " +
//"VALUES('Liu', 'David', 25); ";
