using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public partial class RobotContext : DbContext
    {
        public virtual DbSet<Map> Maps { get; set; } = null!;
        public virtual DbSet<RobotCommand> RobotCommands { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        public RobotContext()
        {
        }

        public RobotContext(DbContextOptions<RobotContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=sit331;Username=postgres;Password=lam789123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("map");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Columns).HasColumnName("columns");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(800)
                    .HasColumnName("description");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modified_date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Rows).HasColumnName("rows");
            });

            modelBuilder.Entity<RobotCommand>(entity =>
            {
                entity.ToTable("robot_command");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(800)
                    .HasColumnName("description");

                entity.Property(e => e.IsMovecommand).HasColumnName("is_movecommand");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modified_date");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("firstname");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifieddate");

                entity.Property(e => e.Passwordhash)
                    .HasMaxLength(255)
                    .HasColumnName("passwordhash");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
