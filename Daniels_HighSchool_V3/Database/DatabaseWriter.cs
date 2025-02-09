using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.Database
{
    public class DatabaseWriter
    {
        private readonly string _connectionString = "Data Source = localhost; Database = Daniels_Highschool_DB_V3; Integrated Security = True; Trust Server Certificate = True;";

        public int Write(string query, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                conn.Open();

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
