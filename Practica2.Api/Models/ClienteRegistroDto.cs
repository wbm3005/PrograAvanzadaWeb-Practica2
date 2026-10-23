using System.ComponentModel.DataAnnotations;

namespace Practica2.Api.Models;

public sealed class ClienteRegistroDto
{
    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(50, ErrorMessage = "La cédula no puede superar los 50 caracteres.")]
    public string? Cedula { get; init; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string? Nombre { get; init; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string? Correo { get; init; }
}
