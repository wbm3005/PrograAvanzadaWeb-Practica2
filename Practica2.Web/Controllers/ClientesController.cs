using Microsoft.AspNetCore.Mvc;
using Practica2.Web.Models;
using Practica2.Web.Services;

namespace Practica2.Web.Controllers;

public sealed class ClientesController(IClientesService clientesService, ILogger<ClientesController> logger) : Controller
{
    [HttpGet]
    public IActionResult Registro() => View(new ClienteRegistroViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(ClienteRegistroViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        try
        {
            var resultado = await clientesService.RegistrarAsync(modelo, cancellationToken);
            if (resultado.Exito)
            {
                TempData["Exito"] = resultado.Mensaje;
                return RedirectToAction(nameof(Registro));
            }

            ModelState.AddModelError(string.Empty, resultado.Mensaje);
        }
        catch (ApiNoDisponibleException ex)
        {
            logger.LogError(ex, "No se pudo registrar el cliente con cédula {Cedula}.", modelo.Cedula);
            ModelState.AddModelError(string.Empty, ApiNoDisponibleException.MensajeUsuario);
        }

        return View(modelo);
    }
}
