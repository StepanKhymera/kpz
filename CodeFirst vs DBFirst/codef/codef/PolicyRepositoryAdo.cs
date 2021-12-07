using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace codef
{
    public class PolicyRepositoryAdo : IPolicyRepository
    {
        private const string ConnectionString = "Data Source=DESKTOP-4IJ8AA6;Integrated Security=True";

        public Policy Add(Policy policy)
        {
            var query = $" INSERT INTO [dbo].[Policies]   VALUES  ( '-' ,  '-',  '-', GETDATE(), GETDATE(), 0,  '-', 0) ";

            ExecuteUpdateQuery(query);

            var result = GetLastAdded();

            return result;
        }

        public void Delete(int id)
        {
            var query = $"Delete from Policies where PolicyID={id} ";

            ExecuteUpdateQuery(query);
        }

        public Policy Get(int id)
        {
            var query = $"Select * from Policies where PolicyID={id} ";

            var result = ExecuteGetQuery(query).FirstOrDefault();

            return result;

        }

        public Policy GetLastAdded()
        {
            //var query = $"Select * from Policies where name='{value}' ";
            var query = "SELECT TOP 1 * FROM Policies ORDER BY PolicyID DESC";

            var result = ExecuteGetQuery(query).FirstOrDefault();
            
            return result;
        }

        public List<Policy> GetAll()
        {
            var query = "Select * from Policies";

            var result = ExecuteGetQuery(query);

            return result;
        }

        public Policy Update(Policy policy)
        {
            var query = $"UPDATE Policies " +
                        $"SET Customer_Name='{policy.Customer_Name}'," +
                            $"Employee_Name = '{policy.Employee_Name}', " +
                            $"Insurance_Type = '{policy.Insurance_Type}', " +
                            $"Policy_Start_Date = '{policy.Policy_Start_Date}', " +
                            $"Policy_Expiration_Date = '{policy.Policy_Expiration_Date}', " +
                            $"Anual_Fee = {policy.Anual_Fee}, " +
                            $"Info_About = '{policy.Info_About}', " +
                            $"Coverage = {policy.Coverage} " +
                        $"WHERE PolicyID={policy.PolicyID}";

            ExecuteUpdateQuery(query);

            var result = Get(policy.PolicyID);

            return result;
        }
 



        private List<Policy> ExecuteGetQuery(string query)
        {

            var results = new List<Policy>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var policyEntity = new Policy
                        {
                            PolicyID = (int)reader[0],
                            Customer_Name = (string)reader[1],
                            Employee_Name = (string)reader[2],
                            Insurance_Type = (string)reader[3],
                            Policy_Start_Date = (DateTime)reader[4],
                            Policy_Expiration_Date = (DateTime)reader[5],
                            Anual_Fee = (decimal)reader[6],
                            Info_About = (string)reader[7],
                            Coverage = (decimal)reader[8]

                        };

                        results.Add(policyEntity);
                    }
                    reader.Close();

                    connection.Close();

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return results;

        }

        private static void ExecuteUpdateQuery(string query)
        {
            using (SqlConnection connection =
                       new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void SaveChanges()
        {

        }

        public List<Policy> GetAllChanged()
        {
            throw new NotImplementedException();
        }
    }
}
