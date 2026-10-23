using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Practica2.Web.Models;

public sealed class MascotaRegistroViewModel
{
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los {1} caracteres.")]
    public string? Nombre { get; set; }

    [Display(Name = "Especie")]
    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(100, ErrorMessage = "La especie no puede superar los {1} caracteres.")]
    public string? Especie { get; set; }

    [Display(Name = "Raza")]
    [Required(ErrorMessage = "La raza es obligatoria.")]
    [StringLength(100, ErrorMessage = "La raza no puede superar los {1} caracteres.")]
    public string? Raza { get; set; }

    // Límites de la columna DECIMAL(8,2).
    [Display(Name = "Peso (kg)")]
    [Required(ErrorMessage = "El peso es obligatorio.")]
    [Range(typeof(decimal), "0.01", "999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true,
        ErrorMessage = "El peso debe estar entre 0.01 y 999999.99.")]
    public decimal? Peso { get; set; }

    [Display(Name = "Cliente")]
    [Required(ErrorMessage = "Seleccione un cliente.")]
    public long? IdCliente { get; set; }

    [ValidateNever]
    public IReadOnlyList<SelectListItem> Clientes { get; set; } = [];

    /// <summary>False cuando el API no respondió; evita mostrar el aviso de "no hay clientes".</summary>
    [ValidateNever]
    public bool ClientesCargados { get; set; }
}
