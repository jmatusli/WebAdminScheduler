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
        public  virtual DbSet<CP_REGISTRO> CP_REGISTRO { get; set; } = null!;
        public  virtual DbSet<CP_CONEXION> CP_CONEXION { get; set; } = null!;
        public  virtual DbSet<CP_DEPENDENCIAS> CP_DEPENDENCIAS { get; set; } = null!;
        public  virtual DbSet<CP_NOTIFICATIONS> CP_NOTIFICATIONS { get; set; } = null!;
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

            modelBuilder.Entity<CP_NOTIFICATIONS>(entity =>
            {
                entity.HasKey(e => e.IDNOTIF)
                 .HasName("PK__Procesos__CRSFDF");

                entity.ToTable("CP_NOTIFICATIONS");

                entity.Property(e => e.RECIPIENTS)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                  
                entity.Property(e => e.NOTIFYSUCCESS)
                    .IsUnicode(false);
                    
                entity.Property(e => e.NOTIFYFAILURE)
                    .IsUnicode(false);
                    
                entity.Property(e => e.NAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CP_REGISTRO>(entity =>
            {
                entity.HasKey(e => e.IDREG)
                 .HasName("PK__Reg__CRSFDF");

                entity.ToTable("CP_REGISTRO");

                entity.Property(e => e.IDPROC)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_INICIO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_EJECUCION)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_FINALIZO)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.ESTADO)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FLAG_ALARMA)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CP_REGISTRO>(entity =>
            {
                entity.HasKey(e => e.IDREG)
                 .HasName("PK__Crontab__CRSFDF");

                entity.ToTable("CP_REGISTRO");

                entity.Property(e => e.IDPROC)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_INICIO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_EJECUCION)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.FEC_FINALIZO)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.ESTADO)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FLAG_ALARMA)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CP_DEPENDENCIAS>(entity =>
            { 
                entity.HasKey(e => e.IDDEP)
                 .HasName("PK_DEPENDENCIAS__CRSFDF");

                entity.ToTable("CP_DEPENDENCIAS");

                entity.Property(e => e.IDPROC)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.IDPROC_DEP)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CP_CONEXION>(entity =>
            { 
                entity.HasKey(e => e.IDCONEX)
                 .HasName("PK__CONEXION__CRSFDF");

                entity.ToTable("CP_CONEXION");

                entity.Property(e => e.USUARIO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.PASSWORD)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.SERVICIO)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.DBLINK)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.TIPO)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}