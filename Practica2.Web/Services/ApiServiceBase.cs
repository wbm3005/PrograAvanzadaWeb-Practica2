using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Practica2.Web.Services;

/// <summary>Llamadas comunes al API: consultas de listas y registros que devuelven 201, 400/409 (ProblemDetails) u otro error.</summary>
public abstract class ApiServiceBase(HttpClient httpClient)
{
    protected async Task<IReadOnlyList<T>> ObtenerListaAsync<T>(string ruta, CancellationToken cancellationToken)
    {
        try
        {
            var datos = await httpClient.GetFromJsonAsync<List<T>>(ruta, cancellationToken);
            return datos ?? [];
        }
        catch (Exception ex) when (EsFalloDeComunicacion(ex, cancellationToken))
        {
            throw new ApiNoDisponibleException($"Falló GET {ruta}.", ex);
        }
    }

    protected async Task<ResultadoRegistro> EnviarRegistroAsync(string ruta, object cuerpo, CancellationToken cancellationToken)
    {
        try
        {
            using var respuesta = await httpClient.PostAsJsonAsync(ruta, cuerpo, cancellationToken);

            if (respuesta.StatusCode == HttpStatusCode.Created)
            {
                var creado = await respuesta.Content.ReadFromJsonAsync<RegistroRespuesta>(cancellationToken);
                return new ResultadoRegistro(true, creado?.Mensaje ?? "Registro completado.");
            }

            if (respuesta.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Conflict)
            {
                var problema = await respuesta.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken);
                return new ResultadoRegistro(false, MensajeDeProblema(problema));
            }

            throw new HttpRequestException($"El API respondió {(int)respuesta.StatusCode}.", null, respuesta.StatusCode);
        }
        catch (Exception ex) when (EsFalloDeComunicacion(ex, cancellationToken))
        {
            throw new ApiNoDisponibleException($"Falló POST {ruta}.", ex);
        }
    }

    // El mensaje del SP llega en Detail; los errores de validación automática del API llegan en Errors.
    private static string MensajeDeProblema(ValidationProblemDetails? problema)
    {
        if (!string.IsNullOrWhiteSpace(problema?.Detail))
        {
            return problema.Detail;
        }

        var errores = problema?.Errors.SelectMany(e => e.Value).ToList() ?? [];
        return errores.Count > 0 ? string.Join(" ", errores) : "El API rechazó los datos enviados.";
    }

    // Un timeout del HttpClient llega como TaskCanceledException sin que el usuario haya cancelado.
    private static bool EsFalloDeComunicacion(Exception ex, CancellationToken cancellationToken) =>
        ex is HttpRequestException or JsonException
        || (ex is TaskCanceledException && !cancellationToken.IsCancellationRequested);

    private sealed record RegistroRespuesta(long Id, string Mensaje);
}
