using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataUploader
{
    public struct DataStruct
    {
        public DateTime dateTime;
        public float acceleration;
    }

    public class DatabaseManager
    {
        SqlConnection connection;

        public DatabaseManager()
        {
            string connectionString = "Data Source=zhengyl-db.database.windows.net; Initial Catalog=zhengyl-db; User ID=liuzhengyang183; Password=COOL_man183";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void Upsert(string tableName, DateTime dateTime, float acceleration)
        {
            string query =string.Format(
                    @"  IF EXISTS(SELECT * FROM {0} WHERE time = @Datetime)
                        UPDATE {0} 
                        SET acceleration = @Acceleration
                        WHERE time = @Datetime
                    ELSE
                        INSERT INTO {0}(time, acceleration) VALUES(@Datetime, @Acceleration);", tableName);

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Datetime", dateTime);
                command.Parameters.AddWithValue("@Acceleration", acceleration);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error has occupied during Upsert of table: {0}, dateTime: {1}, acceleration: {2}", tableName, dateTime, acceleration);
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateTable(string tableName)
        {
            string queryString =
            "CREATE TABLE " + tableName + " (" +
            "time DateTime NOT NULL,    " +
            "acceleration float NOT NULL,    " +
            "PRIMARY KEY(time)); ";

            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
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
