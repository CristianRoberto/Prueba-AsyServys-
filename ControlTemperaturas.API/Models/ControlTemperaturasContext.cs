using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ControlTemperaturas.API.Models
{
    public partial class ControlTemperaturasContext : DbContext
    {
        public ControlTemperaturasContext()
        {
        }

        public ControlTemperaturasContext(DbContextOptions<ControlTemperaturasContext> options)
            : base(options)
        {
        }

        // DbSets principales
        public virtual DbSet<ControlDetalle> ControlDetalles { get; set; }
        public virtual DbSet<ControlEncabezado> ControlEncabezados { get; set; }
        public virtual DbSet<VistaControlTemperatura> VistaControlTemperaturas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Normalmente vacío, la cadena de conexión se define en Program.cs
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabla Detalle
            modelBuilder.Entity<ControlDetalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle)
                      .HasName("PK__ControlDetalle");

                entity.HasOne(d => d.Encabezado) // 👈 Propiedad de navegación limpia
                      .WithMany(p => p.ControlDetalles)
                      .HasForeignKey(d => d.IdControl)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ControlDetalle_ControlEncabezado");
            });

            // Tabla Encabezado
            modelBuilder.Entity<ControlEncabezado>(entity =>
            {
                entity.HasKey(e => e.IdControl)
                      .HasName("PK__ControlEncabezado");

                entity.Property(e => e.FechaRegistro)
                      .HasDefaultValueSql("(sysutcdatetime())");
            });

            // Vista
            modelBuilder.Entity<VistaControlTemperatura>(entity =>
            {
                entity.ToView("Vista_ControlTemperaturas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
