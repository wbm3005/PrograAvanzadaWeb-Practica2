using Microsoft.AspNetCore.Mvc;
using Practica2.Api.Models;

namespace Practica2.Api.Controllers;

/// <summary>Traduce el Codigo devuelto por los procedimientos de registro a una respuesta HTTP.</summary>
public abstract class RegistroControllerBase : ControllerBase
{
    protected IActionResult ResultadoRegistro(ResultadoSp resultado) => resultado.Codigo switch
    {
        ResultadoSp.Ok when resultado.Id is long id =>
            StatusCode(StatusCodes.Status201Created, new RegistroRespuesta(id, resultado.Mensaje)),
        ResultadoSp.CamposInvalidos or ResultadoSp.ClienteNoExiste =>
            Problem(detail: resultado.Mensaje, statusCode: StatusCodes.Status400BadRequest),
        ResultadoSp.CedulaRepetida or ResultadoSp.LimiteEspecie =>
            Problem(detail: resultado.Mensaje, statusCode: StatusCodes.Status409Conflict),
        // Un código fuera del contrato es un error del servidor; lo atiende el manejador global.
        _ => throw new InvalidOperationException($"Código de procedimiento no reconocido: {resultado.Codigo}.")
    };
}
