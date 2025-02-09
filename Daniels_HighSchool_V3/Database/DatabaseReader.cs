using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.Database
{
    internal class DatabaseReader
    {
        private readonly string _connectionString = "Data Source = localhost; Database = Daniels_Highschool_DB_V3; Integrated Security = True; Trust Server Certificate = True;";
        public List<T> Read<T>(string query, Func<SqlDataReader, T> mapToObject, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(mapToObject(reader));
                }
            }
            return result;
        }
    }
}
