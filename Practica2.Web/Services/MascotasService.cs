using Practica2.Web.Models;

namespace Practica2.Web.Services;

public sealed class MascotasService(HttpClient httpClient) : ApiServiceBase(httpClient), IMascotasService
{
    private const string Ruta = "api/mascotas";

    public Task<IReadOnlyList<MascotaConsulta>> ConsultarAsync(CancellationToken cancellationToken) =>
        ObtenerListaAsync<MascotaConsulta>(Ruta, cancellationToken);

    public Task<ResultadoRegistro> RegistrarAsync(MascotaRegistroViewModel mascota, CancellationToken cancellationToken) =>
        EnviarRegistroAsync(Ruta,
            new { mascota.Nombre, mascota.Especie, mascota.Raza, mascota.Peso, mascota.IdCliente },
            cancellationToken);
}
