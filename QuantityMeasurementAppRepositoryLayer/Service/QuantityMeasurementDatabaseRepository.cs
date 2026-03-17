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
                            (Operand1Value, Operand1Unit, Operand2Value, Operand2Unit, Operation, ResultValue)
                            VALUES
                            (@operand1Value, @operand1Unit, @operand2Value, @operand2Unit, @operation, @resultValue)";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@operand1Value", entity.Operand1!.Value);
            cmd.Parameters.AddWithValue("@operand1Unit",  entity.Operand1!.Unit);
            cmd.Parameters.AddWithValue("@operand2Value", entity.Operand2!.Value);
            cmd.Parameters.AddWithValue("@operand2Unit",  entity.Operand2!.Unit);
            cmd.Parameters.AddWithValue("@operation",     entity.Operation);
            cmd.Parameters.AddWithValue("@resultValue",   entity.Result!.Value);

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
                    Operand1  = new QuantityDTO(reader.GetDouble(1), reader.GetString(2)),
                    Operand2  = new QuantityDTO(reader.GetDouble(3), reader.GetString(4)),
                    Operation = reader.GetString(5),
                    Result    = new QuantityDTO(reader.GetDouble(6), reader.GetString(4))
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