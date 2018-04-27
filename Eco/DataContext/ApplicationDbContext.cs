using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Eco.DataContext
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<BranchEmployee> BranchEmployee { get; set; }
        public virtual DbSet<BranchLocation> BranchLocation { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyBranch> CompanyBranch { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<StationActivity> StationActivity { get; set; }
        public virtual DbSet<StationLocation> StationLocation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<BranchEmployee>(entity =>
            {
                entity.HasOne(d => d.FkBranch)
                    .WithMany(p => p.BranchEmployee)
                    .HasForeignKey(d => d.FkBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_BranchEmployee_Branch_FkBranchId");
            });

            modelBuilder.Entity<BranchLocation>(entity =>
            {
                entity.HasOne(d => d.FkBranch)
                    .WithMany(p => p.BranchLocation)
                    .HasForeignKey(d => d.FkBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_BranchLocation_Branch_FkBranchId");

                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.BranchLocation)
                    .HasForeignKey(d => d.FkLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_BranchLocation_Location_LocationId");
            });

            modelBuilder.Entity<CompanyBranch>(entity =>
            {
                entity.HasOne(d => d.FkBranch)
                    .WithMany(p => p.CompanyBranch)
                    .HasForeignKey(d => d.FkBranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_CompanyBranch_Branch_FkBranchId");

                entity.HasOne(d => d.FkCompany)
                    .WithMany(p => p.CompanyBranch)
                    .HasForeignKey(d => d.FkCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_CompanyBranch_Company_FkCompanyId");
            });

            modelBuilder.Entity<StationActivity>(entity =>
            {
                entity.HasOne(d => d.FkActivity)
                    .WithMany(p => p.StationActivity)
                    .HasForeignKey(d => d.FkActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StationActivity_Activity_FkActivityId");

                entity.HasOne(d => d.FkStation)
                    .WithMany(p => p.StationActivity)
                    .HasForeignKey(d => d.FkStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StationActivity_Station_FkStationId");
            });

            modelBuilder.Entity<StationLocation>(entity =>
            {
                entity.HasOne(d => d.FkBranchLocation)
                    .WithMany(p => p.StationLocation)
                    .HasForeignKey(d => d.FkBranchLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StationLocation_BranchLocation_FkBranchLocationId");

                entity.HasOne(d => d.FkStation)
                    .WithMany(p => p.StationLocation)
                    .HasForeignKey(d => d.FkStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StatinoLocation_Station_FkStationId");
            });
        }
    }
}
