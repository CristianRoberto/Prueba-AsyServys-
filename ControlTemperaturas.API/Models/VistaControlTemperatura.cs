using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ControlTemperaturas.API.Models;

[Keyless]
public partial class VistaControlTemperatura
{
    public int IdControl { get; set; }

    [StringLength(120)]
    public string Destino { get; set; } = null!;

    public DateOnly FechaDescongelacion { get; set; }

    public DateOnly FechaProduccion { get; set; }

    public int NumeroCoche { get; set; }

    [StringLength(50)]
    public string CodigoProducto { get; set; } = null!;

    public TimeOnly? HoraInicioDescongelado { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? TemperaturaProducto { get; set; }

    public TimeOnly? HoraInicioConsumo { get; set; }

    public TimeOnly? HoraFinConsumo { get; set; }

    [StringLength(250)]
    public string? Observaciones { get; set; }

    [StringLength(120)]
    public string? RealizadoPor { get; set; }

    [StringLength(120)]
    public string? RevisadoPor { get; set; }

    public DateTime FechaRegistro { get; set; }
}
