using System.Data;
using Dapper;
using Practica2.Api.Data;
using Practica2.Api.Models;

namespace Practica2.Api.Repositories;

public sealed class MascotaRepository(IDbConnectionFactory connectionFactory) : IMascotaRepository
{
    public async Task<IReadOnlyList<MascotaConsulta>> ConsultarAsync(CancellationToken cancellationToken)
    {
        using var conexion = connectionFactory.CrearConexion();
        var comando = new CommandDefinition("dbo.usp_Mascotas_Consultar",
            commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);

        var mascotas = await conexion.QueryAsync<MascotaConsulta>(comando);
        return mascotas.AsList();
    }

    public async Task<ResultadoSp> RegistrarAsync(MascotaRegistroDto mascota, CancellationToken cancellationToken)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@Nombre", mascota.Nombre, DbType.AnsiString, size: 100);
        parametros.Add("@Especie", mascota.Especie, DbType.AnsiString, size: 100);
        parametros.Add("@Raza", mascota.Raza, DbType.AnsiString, size: 100);
        parametros.Add("@Peso", mascota.Peso, DbType.Decimal, precision: 8, scale: 2);
        parametros.Add("@IdCliente", mascota.IdCliente, DbType.Int64);

        using var conexion = connectionFactory.CrearConexion();
        var comando = new CommandDefinition("dbo.usp_Mascotas_Registrar", parametros,
            commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);

        return await conexion.QuerySingleAsync<ResultadoSp>(comando);
    }
}
