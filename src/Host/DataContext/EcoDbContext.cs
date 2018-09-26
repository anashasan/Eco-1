using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Host.DataContext
{
    public partial class EcoDbContext : DbContext
    {
        public EcoDbContext(DbContextOptions<EcoDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<ActivityObservation> ActivityObservation { get; set; }
        public virtual DbSet<ActivityPerform> ActivityPerform { get; set; }
        public virtual DbSet<ActivityPerformDetail> ActivityPerformDetail { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
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
        public virtual DbSet<ClientCompany> ClientCompany { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyBranch> CompanyBranch { get; set; }
        public virtual DbSet<EmployeeProfile> EmployeeProfile { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<JsonData> JsonData { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<StationActivity> StationActivity { get; set; }
        public virtual DbSet<StationLocation> StationLocation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasOne(d => d.FkActivityType)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.FkActivityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Activity_ActivityType_FkActivityTypeId");
            });

            modelBuilder.Entity<ActivityObservation>(entity =>
            {
                entity.HasOne(d => d.FkActivityPerformDetail)
                    .WithMany(p => p.ActivityObservation)
                    .HasForeignKey(d => d.FkActivityPerformDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ActivityObservation_ActivityPerformDetail_FkActivityPerformDetailId");
            });

            modelBuilder.Entity<ActivityPerform>(entity =>
            {
                entity.HasOne(d => d.FkEmployee)
                    .WithMany(p => p.ActivityPerform)
                    .HasForeignKey(d => d.FkEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ActivityPerform_AspNetUsers_FkEmployeeId");

                entity.HasOne(d => d.FkStationLocation)
                    .WithMany(p => p.ActivityPerform)
                    .HasForeignKey(d => d.FkStationLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ActivityPerform_StationLocation_FkStationLocationId");
            });

            modelBuilder.Entity<ActivityPerformDetail>(entity =>
            {
                entity.HasOne(d => d.FkActivity)
                    .WithMany(p => p.ActivityPerformDetail)
                    .HasForeignKey(d => d.FkActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ActivityPerformDetail_Activity_FkActivityId");

                entity.HasOne(d => d.FkActivityPerform)
                    .WithMany(p => p.ActivityPerformDetail)
                    .HasForeignKey(d => d.FkActivityPerformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ActivityPerformDetail_ActivityPerfrom_FkActivityPerformId");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
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

            modelBuilder.Entity<ClientCompany>(entity =>
            {
                entity.Property(e => e.PkClientCompanyId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.FkCompany)
                    .WithMany(p => p.ClientCompany)
                    .HasForeignKey(d => d.FkCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ClientCompany_Company_FkCompanyId");

                entity.HasOne(d => d.FkEmployee)
                    .WithMany(p => p.ClientCompany)
                    .HasForeignKey(d => d.FkEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FkUser_Company_AspNetUsers_FkUserId");
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

            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FkGender)
                    .WithMany(p => p.EmployeeProfile)
                    .HasForeignKey(d => d.FkGenderId)
                    .HasConstraintName("FK__EmployeeProfile_Gender_FkGenderId");

                entity.HasOne(d => d.FkInitiatedBy)
                    .WithMany(p => p.EmployeeProfileFkInitiatedBy)
                    .HasForeignKey(d => d.FkInitiatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.PkGenderId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<JsonData>(entity =>
            {
                entity.Property(e => e.GeneratedCode).HasDefaultValueSql("(newid())");
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
                entity.HasOne(d => d.FkLocation)
                    .WithMany(p => p.StationLocation)
                    .HasForeignKey(d => d.FkLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StationLocation_Location_FkLocationId");

                entity.HasOne(d => d.FkStation)
                    .WithMany(p => p.StationLocation)
                    .HasForeignKey(d => d.FkStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_StatinoLocation_Station_FkStationId");
            });
        }
    }
}
