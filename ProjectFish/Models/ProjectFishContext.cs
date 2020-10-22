using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectFish.Models
{
    public partial class ProjectFishContext : DbContext
    {
        public ProjectFishContext()
        {
        }

        public ProjectFishContext(DbContextOptions<ProjectFishContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Attraction> Attraction { get; set; }
        public virtual DbSet<CompFish> CompFish { get; set; }
        public virtual DbSet<CompLure> CompLure { get; set; }
        public virtual DbSet<CompPlace> CompPlace { get; set; }
        public virtual DbSet<Composition> Composition { get; set; }
        public virtual DbSet<Fish> Fish { get; set; }
        public virtual DbSet<Habitat> Habitat { get; set; }
        public virtual DbSet<Lure> Lure { get; set; }
        public virtual DbSet<Place> Place { get; set; }
        public virtual DbSet<Reel> Reel { get; set; }
        public virtual DbSet<Rod> Rod { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjectFish;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Mail)
                    .HasName("UQ__Account__2724B2D1B284ADA7")
                    .IsUnique();

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Attraction>(entity =>
            {
                entity.HasOne(d => d.Fish)
                    .WithMany(p => p.Attraction)
                    .HasForeignKey(d => d.FishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attraction_Fish");

                entity.HasOne(d => d.Lure)
                    .WithMany(p => p.Attraction)
                    .HasForeignKey(d => d.LureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attraction_Lure");
            });

            modelBuilder.Entity<CompFish>(entity =>
            {
                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.CompFish)
                    .HasForeignKey(d => d.CompositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompFish_Composition");

                entity.HasOne(d => d.Fish)
                    .WithMany(p => p.CompFish)
                    .HasForeignKey(d => d.FishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompFish_Fish");
            });

            modelBuilder.Entity<CompLure>(entity =>
            {
                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.CompLure)
                    .HasForeignKey(d => d.CompositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompLure_Composition");

                entity.HasOne(d => d.Lure)
                    .WithMany(p => p.CompLure)
                    .HasForeignKey(d => d.LureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompLure_Lure");
            });

            modelBuilder.Entity<CompPlace>(entity =>
            {
                entity.Property(e => e.Coordinates)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Composition)
                    .WithMany(p => p.CompPlace)
                    .HasForeignKey(d => d.CompositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompPlace_Composition");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.CompPlace)
                    .HasForeignKey(d => d.Coordinates)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompPlace_Place");
            });

            modelBuilder.Entity<Composition>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Composition_Account");

                entity.HasOne(d => d.Reel)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.ReelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Composition_Reel");

                entity.HasOne(d => d.Rod)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.RodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Composition_Rod");
            });

            modelBuilder.Entity<Fish>(entity =>
            {
                entity.Property(e => e.Species).IsRequired();

                entity.Property(e => e.Waters).IsRequired();

                entity.Property(e => e.WikiLink).IsRequired();
            });

            modelBuilder.Entity<Habitat>(entity =>
            {
                entity.Property(e => e.Coordinates)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CoordinatesNavigation)
                    .WithMany(p => p.Habitat)
                    .HasForeignKey(d => d.Coordinates)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Habitat_Place");

                entity.HasOne(d => d.Fish)
                    .WithMany(p => p.Habitat)
                    .HasForeignKey(d => d.FishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Habitat_Fish");
            });

            modelBuilder.Entity<Lure>(entity =>
            {
                entity.Property(e => e.Brand).IsRequired();

                entity.Property(e => e.Color).IsRequired();

                entity.Property(e => e.Type).IsRequired();
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.Coordinates);

                entity.Property(e => e.Coordinates).HasMaxLength(100);
            });

            modelBuilder.Entity<Reel>(entity =>
            {
                entity.Property(e => e.Brand).IsRequired();

                entity.Property(e => e.Line).IsRequired();

                entity.Property(e => e.Type).IsRequired();
            });

            modelBuilder.Entity<Rod>(entity =>
            {
                entity.Property(e => e.Brand).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
