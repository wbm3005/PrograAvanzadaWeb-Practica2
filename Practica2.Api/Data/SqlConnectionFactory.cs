using System.Data;
using Microsoft.Data.SqlClient;

namespace Practica2.Api.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("Practica2")
        ?? throw new InvalidOperationException("Falta la cadena de conexión 'Practica2' en appsettings.json.");

    public IDbConnection CrearConexion() => new SqlConnection(_cadenaConexion);
}
