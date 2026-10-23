namespace Practica2.Api.Models;

public sealed class MascotaConsulta
{
    public string CedulaCliente { get; init; } = string.Empty;
    public string NombreCliente { get; init; } = string.Empty;
    public string NombreMascota { get; init; } = string.Empty;
    public string Especie { get; init; } = string.Empty;
    public decimal Peso { get; init; }
}
