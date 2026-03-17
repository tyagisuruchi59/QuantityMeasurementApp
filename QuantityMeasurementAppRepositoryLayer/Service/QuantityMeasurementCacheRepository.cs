using System.Text.Json;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly string _filePath = "quantity_measurements.json";

        private List<QuantityMeasurementEntity> LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<QuantityMeasurementEntity>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json)
                   ?? new List<QuantityMeasurementEntity>();
        }

        private void SaveToFile(List<QuantityMeasurementEntity> list)
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_filePath, json);
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            var list = LoadFromFile();
            list.Add(entity);
            SaveToFile(list);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return LoadFromFile();
        }

        public int GetTotalCount()
        {
            return LoadFromFile().Count;
        }

        public void DeleteAll()
        {
            SaveToFile(new List<QuantityMeasurementEntity>());
        }

        // Returns pending records and clears JSON file
        public List<QuantityMeasurementEntity> FlushToDatabase()
        {
            var pending = LoadFromFile();
            if (pending.Count > 0)
                SaveToFile(new List<QuantityMeasurementEntity>());
            return pending;
        }

        public bool HasPendingData()
        {
            return LoadFromFile().Count > 0;
        }
    }
}