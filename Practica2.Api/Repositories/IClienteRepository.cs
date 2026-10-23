using Practica2.Api.Models;

namespace Practica2.Api.Repositories;

public interface IClienteRepository
{
    Task<IReadOnlyList<Cliente>> ListarAsync(CancellationToken cancellationToken);
    Task<ResultadoSp> RegistrarAsync(ClienteRegistroDto cliente, CancellationToken cancellationToken);
}
