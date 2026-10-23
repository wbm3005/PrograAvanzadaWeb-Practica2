using Practica2.Web.Models;

namespace Practica2.Web.Services;

public interface IClientesService
{
    Task<IReadOnlyList<ClienteResumen>> ListarAsync(CancellationToken cancellationToken);
    Task<ResultadoRegistro> RegistrarAsync(ClienteRegistroViewModel cliente, CancellationToken cancellationToken);
}
