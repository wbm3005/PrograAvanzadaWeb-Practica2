using System.Data;

namespace Practica2.Api.Data;

public interface IDbConnectionFactory
{
    IDbConnection CrearConexion();
}
