using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

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

        public void Insert(string tableName, List<String[]> dataList)
        {
            string query = string.Format(
                    @"INSERT INTO [{0}](time, acceleration) VALUES(@Time, @Acceleration);", tableName);

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Time", SqlDbType.Decimal);
                command.Parameters.Add("@Acceleration", SqlDbType.Decimal);
                foreach (string[] item in dataList)
                {
                    command.Parameters["@Time"].Value = item[0];
                    command.Parameters["@Acceleration"].Value = item[1];
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error has occupied during Upsert of table: [{0}], args: {1}", tableName, item.ToString());
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public void BulkInsert(string tableName, List<String[]> dataList)
        {
            System.Data.DataTable table = new DataTable("ParentTable");

            DataColumn timeColumn = new DataColumn();
            timeColumn.DataType = System.Type.GetType("System.Decimal");
            timeColumn.ColumnName = "time";
            timeColumn.Unique = true;

            DataColumn accelerationColumn = new DataColumn();
            accelerationColumn.DataType = System.Type.GetType("System.Decimal");
            accelerationColumn.ColumnName = "acceleration";

            table.Columns.Add(timeColumn);
            table.Columns.Add(accelerationColumn);

            foreach (string[] item in dataList)
            {
                DataRow row = table.NewRow();
                row["time"] = item[0];
                row["acceleration"] = item[0];
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "[" + tableName + "]";

                try
                {
                    // Write from the source to the destination.
                    bulkCopy.WriteToServer(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void CreateTable(string tableName)
        {
            string queryString =
            "CREATE TABLE [" + tableName + "] (" +
            "time numeric NOT NULL,    " +
            "acceleration numeric NOT NULL,    " +
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