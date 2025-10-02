using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ControlTemperaturas.API.Models;

[Table("ControlEncabezado")]
public partial class ControlEncabezado
{
    [Key]
    public int IdControl { get; set; }

    [StringLength(120)]
    public string Destino { get; set; } = null!;

    public DateOnly FechaDescongelacion { get; set; }

    public DateOnly FechaProduccion { get; set; }

    [StringLength(120)]
    public string? RealizadoPor { get; set; }

    [StringLength(120)]
    public string? RevisadoPor { get; set; }

    public DateTime FechaRegistro { get; set; }

    // 🔹 Relación 1:N -> un Encabezado puede tener muchos Detalles
    public virtual ICollection<ControlDetalle> ControlDetalles { get; set; } = new List<ControlDetalle>();
}
