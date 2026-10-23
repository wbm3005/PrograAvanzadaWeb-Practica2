using System.ComponentModel.DataAnnotations;

namespace Practica2.Web.Models;

public sealed class ClienteRegistroViewModel
{
    [Display(Name = "Cédula")]
    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [StringLength(50, ErrorMessage = "La cédula no puede superar los {1} caracteres.")]
    public string? Cedula { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los {1} caracteres.")]
    public string? Nombre { get; set; }

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede superar los {1} caracteres.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string? Correo { get; set; }
}
