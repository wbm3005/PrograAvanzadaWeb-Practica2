using Practica2.Api.Models;

namespace Practica2.Api.Repositories;

public interface IMascotaRepository
{
    Task<IReadOnlyList<MascotaConsulta>> ConsultarAsync(CancellationToken cancellationToken);
    Task<ResultadoSp> RegistrarAsync(MascotaRegistroDto mascota, CancellationToken cancellationToken);
}
