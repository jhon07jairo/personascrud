using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PersonasCrud.Models
{
    public partial class PersonasContext : DbContext
    {
        public PersonasContext()
        {
        }

        public PersonasContext(DbContextOptions<PersonasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pai> Pais { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<VistaPersonasPorDefecto> VistaPersonasPorDefectos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("server=JHONEQUIPO\\SQLEXPRESS; database=Personas; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pai>(entity =>
            {
                entity.HasKey(e => e.CodigoPais)
                    .HasName("PK__Pais__BA1451F52C7BF7BB");

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NombrePais).HasMaxLength(50);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Persona");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos).HasMaxLength(50);

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.CodigoPaisNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.CodigoPais)
                    .HasConstraintName("FK__Persona__CodigoP__15502E78");
            });

            modelBuilder.Entity<VistaPersonasPorDefecto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaPersonasPorDefecto");

                entity.Property(e => e.Apellidos).HasMaxLength(50);

                entity.Property(e => e.FechaNacimiento).HasMaxLength(4000);

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Sexo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.HasSequence("SecuenciaDocumento").StartsAt(100000);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
