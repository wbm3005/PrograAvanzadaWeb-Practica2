using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Practica2.Web.Models;
using Practica2.Web.Services;

namespace Practica2.Web.Controllers;

public sealed class MascotasController(
    IMascotasService mascotasService,
    IClientesService clientesService,
    ILogger<MascotasController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Registro(CancellationToken cancellationToken)
    {
        var modelo = new MascotaRegistroViewModel();
        await CargarClientesAsync(modelo, cancellationToken);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(MascotaRegistroViewModel modelo, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var resultado = await mascotasService.RegistrarAsync(modelo, cancellationToken);
                if (resultado.Exito)
                {
                    // Se va a la consulta para que el usuario vea la mascota recién agregada en la tabla.
                    TempData["Exito"] = resultado.Mensaje;
                    return RedirectToAction(nameof(Consulta));
                }

                ModelState.AddModelError(string.Empty, resultado.Mensaje);
            }
            catch (ApiNoDisponibleException ex)
            {
                logger.LogError(ex, "No se pudo registrar la mascota del cliente {IdCliente}.", modelo.IdCliente);
                ModelState.AddModelError(string.Empty, ApiNoDisponibleException.MensajeUsuario);
            }
        }

        // La lista no viaja en el POST: sin esto el dropdown queda vacío al volver a mostrar el formulario.
        await CargarClientesAsync(modelo, cancellationToken);
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Consulta(CancellationToken cancellationToken)
    {
        try
        {
            var mascotas = await mascotasService.ConsultarAsync(cancellationToken);
            return View(new MascotasConsultaViewModel { Mascotas = mascotas });
        }
        catch (ApiNoDisponibleException ex)
        {
            logger.LogError(ex, "No se pudo consultar la lista de mascotas.");
            return View(new MascotasConsultaViewModel { Error = ApiNoDisponibleException.MensajeUsuario });
        }
    }

    private async Task CargarClientesAsync(MascotaRegistroViewModel modelo, CancellationToken cancellationToken)
    {
        try
        {
            var clientes = await clientesService.ListarAsync(cancellationToken);
            modelo.Clientes = clientes
                .Select(c => new SelectListItem($"{c.Cedula} – {c.Nombre}", c.IdCliente.ToString()))
                .ToList();
            modelo.ClientesCargados = true;
        }
        catch (ApiNoDisponibleException ex)
        {
            logger.LogError(ex, "No se pudo cargar la lista de clientes.");
            modelo.Clientes = [];
            modelo.ClientesCargados = false;
            // Evita repetir el mensaje si el registro ya falló por la misma causa.
            var yaInformado = ModelState.TryGetValue(string.Empty, out var entrada)
                && entrada.Errors.Any(e => e.ErrorMessage == ApiNoDisponibleException.MensajeUsuario);
            if (!yaInformado)
            {
                ModelState.AddModelError(string.Empty, ApiNoDisponibleException.MensajeUsuario);
            }
        }
    }
}
