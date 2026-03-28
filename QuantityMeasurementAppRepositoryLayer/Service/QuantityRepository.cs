using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Context;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class QuantityRepository : IQuantityMeasurementRepository
    {
        private readonly QuantityDbContext _context;
        private readonly ILogger<QuantityRepository> _logger;

        public QuantityRepository(QuantityDbContext context, ILogger<QuantityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<QuantityMeasurementEntity> SaveAsync(QuantityMeasurementEntity entity)
        {
            try
            {
                _context.QuantityMeasurements.Add(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Saved QuantityMeasurementEntity with Id={Id}", entity.Id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving QuantityMeasurementEntity");
                throw;
            }
        }

        public async Task<List<QuantityMeasurementEntity>> FindAllAsync()
        {
            _logger.LogDebug("Fetching all QuantityMeasurements");
            return await _context.QuantityMeasurements
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<QuantityMeasurementEntity>> FindByOperationAsync(string operation)
        {
            _logger.LogDebug("Fetching by operation={Operation}", operation);
            return await _context.QuantityMeasurements
                .Where(e => e.Operation.ToUpper() == operation.ToUpper())
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<QuantityMeasurementEntity>> FindByMeasurementTypeAsync(string measurementType)
        {
            _logger.LogDebug("Fetching by measurementType={Type}", measurementType);
            return await _context.QuantityMeasurements
                .Where(e => e.ThisMeasurementType == measurementType)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<QuantityMeasurementEntity>> FindByIsErrorTrueAsync()
        {
            _logger.LogDebug("Fetching errored QuantityMeasurements");
            return await _context.QuantityMeasurements
                .Where(e => e.IsError)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<QuantityMeasurementEntity>> FindByCreatedAtAfterAsync(DateTime date)
        {
            return await _context.QuantityMeasurements
                .Where(e => e.CreatedAt > date)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountByOperationAsync(string operation)
        {
            return await _context.QuantityMeasurements
                .CountAsync(e => e.Operation.ToUpper() == operation.ToUpper());
        }

        public async Task<int> CountSuccessfulByOperationAsync(string operation)
        {
            return await _context.QuantityMeasurements
                .CountAsync(e => e.Operation.ToUpper() == operation.ToUpper() && !e.IsError);
        }

        public async Task DeleteAllAsync()
        {
            _context.QuantityMeasurements.RemoveRange(_context.QuantityMeasurements);
            await _context.SaveChangesAsync();
            _logger.LogWarning("All QuantityMeasurements deleted");
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.QuantityMeasurements.CountAsync();
        }
    }
}