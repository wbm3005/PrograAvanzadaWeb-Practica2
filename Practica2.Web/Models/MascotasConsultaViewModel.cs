namespace Practica2.Web.Models;

public sealed class MascotasConsultaViewModel
{
    public IReadOnlyList<MascotaConsulta> Mascotas { get; init; } = [];

    public string? Error { get; init; }
}
