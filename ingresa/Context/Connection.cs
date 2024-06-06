using Sap.Data.Hana;
using System.Data.SqlClient;

namespace ingresa.Context
{
    public class Connection(IConfiguration iConfig)
    {

        private readonly IConfiguration configuration = iConfig;
        public HanaConnection ContextSAP() => new(configuration.GetConnectionString("FemacoSAP"));

    }
}
