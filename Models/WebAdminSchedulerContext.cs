using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAdminScheduler.Models
{
    public partial class WebAdminSchedulerContext : DbContext
    {
        public WebAdminSchedulerContext()
        {
        }
        public WebAdminSchedulerContext(DbContextOptions<WebAdminSchedulerContext> options)
            : base(options)
        {
        }
        public  virtual DbSet<CP_CRONTAB> CP_CRONTABS { get; set; } = null!;
        public  virtual DbSet<CP_PROCESOS> CP_PROCESOS { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CP_CRONTAB>(entity =>
            {
                entity.HasKey(e => e.IDCRONTAB)
                 .HasName("PK__Crontab__CRSFDF");

                entity.ToTable("CP_CRONTAB");

                entity.Property(e => e.HORA_INICIO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.HORA_FIN)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.RECURRENCIA)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                     entity.Property(e => e.WDAY_M2S_EX)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                    entity.Property(e => e.DAY_EX)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                    entity.Property(e => e.MONTH_EX)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                    entity.Property(e => e.REPEAT_EVERY_MINS)
                    .IsUnicode(false);

                    entity.Property(e => e.REPEAT_AFTER_FINISH)
                    .IsUnicode(false);
                 
            });

            modelBuilder.Entity<CP_PROCESOS>(entity =>
            {
                entity.HasKey(e => e.IDPROC)
                 .HasName("PK__Procesos__CRSFDF");

                entity.ToTable("CP_PROCESOS");

                entity.Property(e => e.IDCONEX)
                    .IsUnicode(false);
                    
                entity.Property(e => e.NOMBRE)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                    
                entity.Property(e => e.DESCRIPCION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PATH)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PARAMETRO1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PARAMETRO2)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PARAMETRO3)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PARAMETRO4)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DEPENDENCIA)
                    .IsUnicode(false);

                entity.Property(e => e.INTENTOS)
                    .IsUnicode(false);
                    
                entity.Property(e => e.ESPERA_INTENTO)
                    .IsUnicode(false);

                entity.Property(e => e.ESTADO)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FTP)
                    .IsUnicode(false);
                    
                entity.Property(e => e.IDHOST)
                    .IsUnicode(false);
                    
                entity.Property(e => e.COMPRESION)
                    .IsUnicode(false);

                entity.Property(e => e.IDCRONTAB)
                    .IsUnicode(false);
                    
                entity.Property(e => e.NODE)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}