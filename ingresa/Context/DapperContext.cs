using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System.Configuration;
namespace ingresa.Context
{

    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection() => new(_configuration.GetConnectionString("Connection"));
        public HanaConnection CreateConnectionSAP() => new(_configuration.GetConnectionString("FemacoSAP"));
    }
}