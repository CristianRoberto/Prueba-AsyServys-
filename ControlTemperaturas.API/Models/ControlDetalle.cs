using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // 👈 importar

namespace ControlTemperaturas.API.Models
{
    [Table("ControlDetalle")]
    public partial class ControlDetalle
    {
        [Key]
        public int IdDetalle { get; set; }

        public int IdControl { get; set; }

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

        // 🔹 Navegación hacia Encabezado (se ignora en JSON para evitar ciclos)
        [ForeignKey("IdControl")]
        [JsonIgnore] // 👈 evita que vuelva a serializar el encabezado y cause bucle
        public virtual ControlEncabezado? Encabezado { get; set; }
    }
}
