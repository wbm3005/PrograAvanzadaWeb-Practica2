namespace Practica2.Api.Models;

/// <summary>Fila que devuelven los procedimientos de registro (Codigo, Mensaje, Id).</summary>
public sealed class ResultadoSp
{
    public const int Ok = 0;
    public const int CamposInvalidos = 1;
    public const int CedulaRepetida = 2;
    public const int ClienteNoExiste = 3;
    public const int LimiteEspecie = 4;

    public int Codigo { get; init; }
    public string Mensaje { get; init; } = string.Empty;
    public long? Id { get; init; }
}
