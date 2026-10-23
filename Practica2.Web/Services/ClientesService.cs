using Practica2.Web.Models;

namespace Practica2.Web.Services;

public sealed class ClientesService(HttpClient httpClient) : ApiServiceBase(httpClient), IClientesService
{
    private const string Ruta = "api/clientes";

    public Task<IReadOnlyList<ClienteResumen>> ListarAsync(CancellationToken cancellationToken) =>
        ObtenerListaAsync<ClienteResumen>(Ruta, cancellationToken);

    public Task<ResultadoRegistro> RegistrarAsync(ClienteRegistroViewModel cliente, CancellationToken cancellationToken) =>
        EnviarRegistroAsync(Ruta, new { cliente.Cedula, cliente.Nombre, cliente.Correo }, cancellationToken);
}
