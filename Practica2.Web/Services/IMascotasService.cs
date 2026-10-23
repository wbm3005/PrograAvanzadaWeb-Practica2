using Practica2.Web.Models;

namespace Practica2.Web.Services;

public interface IMascotasService
{
    Task<IReadOnlyList<MascotaConsulta>> ConsultarAsync(CancellationToken cancellationToken);
    Task<ResultadoRegistro> RegistrarAsync(MascotaRegistroViewModel mascota, CancellationToken cancellationToken);
}
