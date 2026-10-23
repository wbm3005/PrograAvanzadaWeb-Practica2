namespace Practica2.Web.Services;

/// <summary>El API no respondió o respondió de forma inesperada (caído, timeout, error 500).</summary>
public sealed class ApiNoDisponibleException(string message, Exception innerException)
    : Exception(message, innerException)
{
    public const string MensajeUsuario =
        "No fue posible comunicarse con el servicio en este momento. Intente de nuevo más tarde.";
}
