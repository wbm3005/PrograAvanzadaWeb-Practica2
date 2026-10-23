using System.Data;
using Dapper;
using Practica2.Api.Data;
using Practica2.Api.Models;

namespace Practica2.Api.Repositories;

public sealed class ClienteRepository(IDbConnectionFactory connectionFactory) : IClienteRepository
{
    public async Task<IReadOnlyList<Cliente>> ListarAsync(CancellationToken cancellationToken)
    {
        using var conexion = connectionFactory.CrearConexion();
        var comando = new CommandDefinition("dbo.usp_Clientes_Listar",
            commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);

        var clientes = await conexion.QueryAsync<Cliente>(comando);
        return clientes.AsList();
    }

    public async Task<ResultadoSp> RegistrarAsync(ClienteRegistroDto cliente, CancellationToken cancellationToken)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@Cedula", cliente.Cedula, DbType.AnsiString, size: 50);
        parametros.Add("@Nombre", cliente.Nombre, DbType.AnsiString, size: 100);
        parametros.Add("@Correo", cliente.Correo, DbType.AnsiString, size: 100);

        using var conexion = connectionFactory.CrearConexion();
        var comando = new CommandDefinition("dbo.usp_Clientes_Registrar", parametros,
            commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);

        return await conexion.QuerySingleAsync<ResultadoSp>(comando);
    }
}
