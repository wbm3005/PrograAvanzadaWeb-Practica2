using Microsoft.AspNetCore.Mvc;
using Practica2.Api.Models;
using Practica2.Api.Repositories;

namespace Practica2.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public sealed class ClientesController(IClienteRepository clienteRepository) : RegistroControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<Cliente>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Cliente>>> Listar(CancellationToken cancellationToken)
    {
        var clientes = await clienteRepository.ListarAsync(cancellationToken);
        return Ok(clientes);
    }

    [HttpPost]
    [ProducesResponseType<RegistroRespuesta>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(ClienteRegistroDto cliente, CancellationToken cancellationToken)
    {
        var resultado = await clienteRepository.RegistrarAsync(cliente, cancellationToken);
        return ResultadoRegistro(resultado);
    }
}
