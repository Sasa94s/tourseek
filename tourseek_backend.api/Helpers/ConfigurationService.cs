using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace tourseek_backend.api.Helpers
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const string DATABASE_URL = "DATABASE_URL";
        
        public ConfigurationService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        private string GetHerokuConnectionString() {
            // Get the connection string from the ENV variables
            string connectionUrl = Environment.GetEnvironmentVariable(DATABASE_URL);

            if (connectionUrl == null)
            {
                throw new ArgumentNullException($"{DATABASE_URL} environment variable is unset");
            }
            
            // parse the connection string
            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }

        public string DatabaseConnectionString =>
            _webHostEnvironment.IsDevelopment()
                ? _configuration.GetConnectionString("DefaultConnection")
                : GetHerokuConnectionString();

    }
}