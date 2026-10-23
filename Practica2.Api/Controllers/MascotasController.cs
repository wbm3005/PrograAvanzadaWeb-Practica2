using Microsoft.AspNetCore.Mvc;
using Practica2.Api.Models;
using Practica2.Api.Repositories;

namespace Practica2.Api.Controllers;

[ApiController]
[Route("api/mascotas")]
public sealed class MascotasController(IMascotaRepository mascotaRepository) : RegistroControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<MascotaConsulta>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MascotaConsulta>>> Consultar(CancellationToken cancellationToken)
    {
        var mascotas = await mascotaRepository.ConsultarAsync(cancellationToken);
        return Ok(mascotas);
    }

    [HttpPost]
    [ProducesResponseType<RegistroRespuesta>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(MascotaRegistroDto mascota, CancellationToken cancellationToken)
    {
        var resultado = await mascotaRepository.RegistrarAsync(mascota, cancellationToken);
        return ResultadoRegistro(resultado);
    }
}
