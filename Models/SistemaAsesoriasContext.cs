using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SAFIM.Models;

public partial class SistemaAsesoriasContext : DbContext
{
    public SistemaAsesoriasContext()
    {
    }

    public SistemaAsesoriasContext(DbContextOptions<SistemaAsesoriasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administrador { get; set; }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Asesor> Asesor { get; set; }

    public virtual DbSet<Asesoria> Asesoria { get; set; }

    public virtual DbSet<Detallereporte> Detallereporte { get; set; }

    public virtual DbSet<Materia> Materia { get; set; }

    public virtual DbSet<Reporte> Reporte { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Matricula).HasName("PK_administrador_matricula");

            entity.ToTable("administrador");

            entity.Property(e => e.Matricula)
                .ValueGeneratedNever()
                .HasColumnName("matricula");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.NombreAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreAdmin");
        });

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Matricula).HasName("PK_alumno_matricula");

            entity.ToTable("alumno");

            entity.Property(e => e.Matricula)
                .ValueGeneratedNever()
                .HasColumnName("matricula");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoM");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoP");
            entity.Property(e => e.Carrera)
                .IsUnicode(false)
                .HasColumnName("carrera");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.NombreAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreAlumno");
            entity.Property(e => e.Semestre).HasColumnName("semestre");
        });

        modelBuilder.Entity<Asesor>(entity =>
        {
            entity.HasKey(e => e.Matricula).HasName("PK_asesor_matricula");

            entity.ToTable("asesor");

            entity.Property(e => e.Matricula)
                .ValueGeneratedNever()
                .HasColumnName("matricula");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contraseña");

            entity.HasOne(d => d.MatriculaNavigation).WithOne(p => p.Asesor)
                .HasForeignKey<Asesor>(d => d.Matricula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_asesor_matricula");
        });

        modelBuilder.Entity<Asesoria>(entity =>
        {
            entity.HasKey(e => e.IdAsesoria).HasName("PK_asesoria_idAsesoria");

            entity.ToTable("asesoria");

            entity.Property(e => e.IdAsesoria).HasColumnName("idAsesoria");
            entity.Property(e => e.ClaveMateria).HasColumnName("claveMateria");
            entity.Property(e => e.HorarioJueves)
                .IsUnicode(false)
                .HasColumnName("horarioJueves");
            entity.Property(e => e.HorarioLunes)
                .IsUnicode(false)
                .HasColumnName("horarioLunes");
            entity.Property(e => e.HorarioMartes)
                .IsUnicode(false)
                .HasColumnName("horarioMartes");
            entity.Property(e => e.HorarioMiercoles)
                .IsUnicode(false)
                .HasColumnName("horarioMiercoles");
            entity.Property(e => e.HorarioViernes)
                .IsUnicode(false)
                .HasColumnName("horarioViernes");
            entity.Property(e => e.MatriculaAsesor).HasColumnName("matriculaAsesor");

            entity.HasOne(d => d.ClaveMateriaNavigation).WithMany(p => p.Asesoria)
                .HasForeignKey(d => d.ClaveMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_asesoria_claveMateria");

            entity.HasOne(d => d.MatriculaAsesorNavigation).WithMany(p => p.Asesoria)
                .HasForeignKey(d => d.MatriculaAsesor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_asesoria_matriculaAsesor");
        });

        modelBuilder.Entity<Detallereporte>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK_reporte_idDetalle");

            entity.ToTable("detallereporte");

            entity.Property(e => e.IdDetalle).HasColumnName("idDetalle");
            entity.Property(e => e.Comentarios)
                .IsUnicode(false)
                .HasColumnName("comentarios");
            entity.Property(e => e.Grupo).HasColumnName("grupo");
            entity.Property(e => e.IdReporte).HasColumnName("idReporte");
            entity.Property(e => e.MatriculaAlumno).HasColumnName("matriculaAlumno");
            entity.Property(e => e.ProgramaEducativo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("programaEducativo");
            entity.Property(e => e.Temas)
                .IsUnicode(false)
                .HasColumnName("temas");
            entity.Property(e => e.Tiempo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tiempo");

            entity.HasOne(d => d.IdReporteNavigation).WithMany(p => p.Detallereporte)
                .HasForeignKey(d => d.IdReporte)
                .HasConstraintName("FK_reporte_idreporte");

            entity.HasOne(d => d.MatriculaAlumnoNavigation).WithMany(p => p.Detallereporte)
                .HasForeignKey(d => d.MatriculaAlumno)
                .HasConstraintName("FK_reporte_matriculaAlumno");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.ClaveMateria).HasName("PK_materia_claveMateria");

            entity.ToTable("materia");

            entity.Property(e => e.ClaveMateria)
                .ValueGeneratedNever()
                .HasColumnName("claveMateria");
            entity.Property(e => e.NombreMateria)
                .IsUnicode(false)
                .HasColumnName("nombreMateria");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK_reporte_idReporte");

            entity.ToTable("reporte");

            entity.Property(e => e.IdReporte).HasColumnName("idReporte");
            entity.Property(e => e.ClaveMateria).HasColumnName("claveMateria");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Horario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("horario");
            entity.Property(e => e.IdAsesoria).HasColumnName("idAsesoria");
            entity.Property(e => e.MatriculaAsesor).HasColumnName("matriculaAsesor");

            entity.HasOne(d => d.ClaveMateriaNavigation).WithMany(p => p.Reporte)
                .HasForeignKey(d => d.ClaveMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reporte_matricula");

            entity.HasOne(d => d.MatriculaAsesorNavigation).WithMany(p => p.Reporte)
                .HasForeignKey(d => d.MatriculaAsesor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reporte_matriculaAsesor");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK_rol_idRol");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol)
                .ValueGeneratedNever()
                .HasColumnName("idRol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Matricula).HasName("PK_usuarios_matricula");

            entity.ToTable("usuario");

            entity.Property(e => e.Matricula)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("matricula");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_usuario_idRol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
