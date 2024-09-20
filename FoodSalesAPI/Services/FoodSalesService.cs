using FoodSalesAPI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace FoodSalesAPI.Services
{
    public class FoodSalesService
    {
        private readonly IMongoCollection<FoodSales> _foodSales;

        public FoodSalesService(IConfiguration config)
        {
            var connectionString = config.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = config.GetSection("MongoDB:DatabaseName").Value;

            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                CheckCertificateRevocation = false
            };
            settings.AllowInsecureTls = true;

            var client = new MongoClient(settings);
            var database = client.GetDatabase(databaseName);

            _foodSales = database.GetCollection<FoodSales>("FoodSales");
        }

        public async Task<List<FoodSales>> GetAsync() =>
            await _foodSales.Find(_ => true).ToListAsync();

        public async Task<FoodSales> GetByIdAsync(string id) =>
            await _foodSales.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(FoodSales foodSale) =>
            await _foodSales.InsertOneAsync(foodSale);

        public async Task UpdateAsync(string id, FoodSales foodSale) =>
            await _foodSales.ReplaceOneAsync(x => x.Id == id, foodSale);

        public async Task DeleteAsync(string id) =>
            await _foodSales.DeleteOneAsync(x => x.Id == id);
    }
}
