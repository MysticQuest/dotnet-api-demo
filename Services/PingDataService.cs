using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.Extensions.Logging;
using Models;

namespace Services
{
    public class PingDataService : IService<PingData>
    {
        private readonly IRepository<PingData> _repository;
        private readonly ILogger<PingDataService> _logger;

        public PingDataService(IRepository<PingData> repository, ILogger<PingDataService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PingData>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PingData> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PingData> CreateAsync()
        {
            //test
            var pingData = new PingData
            {
                Domain = "example.com",
                RoundtripTime = 100,
                Status = "Success",
                DateTime = DateTime.UtcNow
            };

            await _repository.CreateAsync(pingData);
            return pingData;
        }

        public async Task UpdateAsync(PingData pingData)
        {
            await _repository.UpdateAsync(pingData);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
