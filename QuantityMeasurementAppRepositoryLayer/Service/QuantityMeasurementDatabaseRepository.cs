using Microsoft.Data.SqlClient;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppModel.DTOs;
using QuantityMeasurementAppRepositoryLayer.Interface;
using QuantityMeasurementAppRepositoryLayer.Utilities;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        public void Save(QuantityMeasurementEntity entity)
        {
            using SqlConnection conn = ConnectionFactory.GetConnection();

            string query = @"INSERT INTO QuantityMeasurements
                            (Operand1,Unit1,Operand2,Unit2,Operation,Result)
                            VALUES
                            (@op1,@u1,@op2,@u2,@operation,@result)";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@op1", entity.Operand1.Value);
            cmd.Parameters.AddWithValue("@u1", entity.Operand1.Unit);

            cmd.Parameters.AddWithValue("@op2", entity.Operand2.Value);
            cmd.Parameters.AddWithValue("@u2", entity.Operand2.Unit);

            cmd.Parameters.AddWithValue("@operation", entity.Operation);

            cmd.Parameters.AddWithValue("@result", entity.Result.Value);

            cmd.ExecuteNonQuery();
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            List<QuantityMeasurementEntity> list = new();

            using SqlConnection conn = ConnectionFactory.GetConnection();

            string query = "SELECT * FROM QuantityMeasurements";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                QuantityMeasurementEntity entity = new QuantityMeasurementEntity
                {
                    Operand1 = new QuantityDTO(reader.GetDouble(1), reader.GetString(2)),
                    Operand2 = new QuantityDTO(reader.GetDouble(3), reader.GetString(4)),
                    Operation = reader.GetString(5),
                    Result = new QuantityDTO(reader.GetDouble(6), reader.GetString(2))
                };

                list.Add(entity);
            }

            return list;
        }

        public int GetTotalCount()
        {
            using SqlConnection conn = ConnectionFactory.GetConnection();

            string query = "SELECT COUNT(*) FROM QuantityMeasurements";

            SqlCommand cmd = new SqlCommand(query, conn);

            return (int)cmd.ExecuteScalar();
        }

        public void DeleteAll()
        {
            using SqlConnection conn = ConnectionFactory.GetConnection();

            string query = "DELETE FROM QuantityMeasurements";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.ExecuteNonQuery();
        }
    }
}