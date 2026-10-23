namespace Practica2.Api.Models;

public sealed class Cliente
{
    public long IdCliente { get; init; }
    public string Cedula { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
}
