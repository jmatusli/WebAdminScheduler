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

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}