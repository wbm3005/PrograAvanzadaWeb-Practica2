using System.ComponentModel.DataAnnotations;

namespace Practica2.Api.Models;

public sealed class MascotaRegistroDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string? Nombre { get; init; }

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(100, ErrorMessage = "La especie no puede superar los 100 caracteres.")]
    public string? Especie { get; init; }

    [Required(ErrorMessage = "La raza es obligatoria.")]
    [StringLength(100, ErrorMessage = "La raza no puede superar los 100 caracteres.")]
    public string? Raza { get; init; }

    // Límites de la columna DECIMAL(8,2).
    [Required(ErrorMessage = "El peso es obligatorio.")]
    [Range(typeof(decimal), "0.01", "999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true,
        ErrorMessage = "El peso debe estar entre 0.01 y 999999.99.")]
    public decimal? Peso { get; init; }

    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public long? IdCliente { get; init; }
}
