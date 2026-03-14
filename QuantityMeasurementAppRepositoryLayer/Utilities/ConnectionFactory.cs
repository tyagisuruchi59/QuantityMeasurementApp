using Microsoft.Data.SqlClient;

namespace QuantityMeasurementAppRepositoryLayer.Utilities
{
    public class ConnectionFactory
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(
                "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            connection.Open();
            return connection;
        }
    }
}