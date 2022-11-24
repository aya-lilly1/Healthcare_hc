using System;
using Healthcare_hc.Models.RoleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Healthcare_hc.Models
{
    public partial class healthcare_hcContext : DbContext
    {
        public healthcare_hcContext()
        {
        }

        public healthcare_hcContext(DbContextOptions<healthcare_hcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<DoctorAppointment> DoctorAppointments { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientAppointment> PatientAppointments { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<UserPermissionView> UserPermissionView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseMySQL("Server=localhost;port=3306;user=root;password=Manager@123456;database=healthcare_hc;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.AdminId, "idadmin_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.AdminId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("adminId");

                entity.HasOne(d => d.AdminNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adminId_userId");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blog");

                entity.HasIndex(e => e.CreatorId, "blog_doctorId_idx");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Archived).HasColumnName("archived");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreatorId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("creatorId");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("image");

                entity.Property(e => e.Status)
                    .HasColumnType("tinyint")
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("title")
                    .HasDefaultValueSql("'\"\"'");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("blog_doctorId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.ToTable("clinic");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Archived).HasColumnName("archived");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.CreatedDate)
                   .HasColumnType("timestamp")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedDate)
                   .HasColumnType("timestamp")
                   .ValueGeneratedOnAddOrUpdate()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctor");

                entity.HasIndex(e => e.ClinicId, "clinicId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DepartmentId, "departmentId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Status, "status_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "userId_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.ClinicId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("clinicId");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("departmentId");

                entity.Property(e => e.DoctorId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("doctorId");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Clinic)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.ClinicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("clinic_doctor");

                entity.HasOne(d => d.Department)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("depatrmentId_doctor");

                entity.HasOne(d => d.DoctorNavigation)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey<Doctor>(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("doctorId_doctor");
            });

            modelBuilder.Entity<DoctorAppointment>(entity =>
            {
                entity.ToTable("doctor_appointment");

                entity.HasIndex(e => e.DoctorId, "doctorId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Archived).HasColumnName("archived");

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("day");

                entity.Property(e => e.DoctorId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("doctorId");

                entity.Property(e => e.EndTime).HasColumnName("endTime");

                entity.Property(e => e.NumberOfPatients).HasColumnName("numberOfPatients");

                entity.Property(e => e.StartTime).HasColumnName("startTime");

                entity.HasOne(d => d.Doctor)
                    .WithOne(p => p.DoctorAppointment)
                    .HasForeignKey<DoctorAppointment>(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("doctorId_Appointment");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PatientId, "patientId_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("patientId");

                entity.HasOne(d => d.PatientNavigation)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Patient>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("patientId_userId");
            });

            modelBuilder.Entity<PatientAppointment>(entity =>
            {
                entity.ToTable("patient_appointment");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "doctorId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "doctorId_appointment_idx");

                entity.HasIndex(e => e.PatientId, "patientId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PatientId, "patientId_appointment_idx");

                entity.Property(e => e.Id).HasColumnType("int unsigned");

                entity.Property(e => e.Archived).HasColumnName("archived");

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("day");

                entity.Property(e => e.DoctorId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("doctorId");

                entity.Property(e => e.PatientId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("patientId");
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedDate)
                   .HasColumnType("timestamp")
                   .ValueGeneratedOnAddOrUpdate()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.Doctor)
                    .WithOne(p => p.PatientAppointment)
                    .HasPrincipalKey<Doctor>(p => p.DoctorId)
                    .HasForeignKey<PatientAppointment>(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("doctor_appointment");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.PatientAppointment)
                    .HasPrincipalKey<Patient>(p => p.PatientId)
                    .HasForeignKey<PatientAppointment>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("patientId_appointment");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("state");

                entity.HasIndex(e => e.CityId, "cityId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.CityId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("cityId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.HasOne(d => d.City)
                    .WithOne(p => p.State)
                    .HasForeignKey<State>(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stateId_ctyId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.CityId, "cityId_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.StateId, "stateId_users_idx");

                entity.Property(e => e.Id)
                    .HasColumnType("int unsigned")
                    .HasColumnName("id");

                entity.Property(e => e.Archived).HasColumnName("archived");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.Address)
                   .IsRequired()
                   .HasMaxLength(300)
                   .HasColumnName("address");

                entity.Property(e => e.CityId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("cityId");

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("confirmPassword");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("firstName")
                    .HasDefaultValueSql("'\"\"'");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("lastName")
                    .HasDefaultValueSql("'\"\"'");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("phone");

               entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.StateId)
                    .HasColumnType("int unsigned")
                    .HasColumnName("stateId");

                entity.HasOne(d => d.City)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ctyId_users");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stateId_users");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId)
                      .HasDatabaseName("RoleId_UserGroupId_idx");

                entity.HasIndex(e => e.UserId)
                      .HasDatabaseName("GroupId_UserGroupId_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.Archived).HasColumnType("tinyint(3)");


                entity.Property(e => e.CreatedUTC)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastUpdatedUTC)
                        .HasColumnType("datetime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Archived)
                      .HasColumnType("tinyint(3)");

                entity.HasOne(d => d.Users)
                      .WithMany(p => p.UserRoles)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("UserRole_UserId");

                entity.HasOne(d => d.Role)
                      .WithMany(p => p.UserRoles)
                      .HasForeignKey(d => d.RoleId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("UserRole_RoleId");

            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasIndex(e => e.Id)
                      .HasDatabaseName("Id_UNIQUE")
                      .IsUnique();

                entity.HasIndex(e => e.RoleId)
                      .HasDatabaseName("RoleId_RolePermission_idx");

                entity.HasIndex(e => e.PermissionId)
                      .HasDatabaseName("PermissionId_UserPermissionId_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");
                entity.Property(e => e.PermissionId).HasColumnType("int(11)");

                entity.Property(e => e.Archived).HasColumnType("tinyint(3)");

                entity.Property(e => e.CreatedUTC)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastUpdatedUTC)
                      .HasColumnType("datetime")
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Archived)
                      .HasColumnType("tinyint(3)");

                entity.HasOne(d => d.Role)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(d => d.RoleId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("RoleId_RolePermission");

                entity.HasOne(d => d.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(d => d.PermissionId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("PermissionId_UserPermissionId");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ModuleId)
                      .HasDatabaseName("ModuleId_PrmessionModuleId_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ModuleId).HasColumnType("int(11)");

                entity.Property(e => e.Archived).HasColumnType("tinyint(3)");

                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasColumnType("TEXT");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasColumnType("varchar(255)");

                entity.Property(e => e.Code)
                      .IsRequired()
                      .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedUTC)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastUpdatedUTC)
                        .HasColumnType("datetime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Archived)
                      .HasColumnType("tinyint(3)");

                entity.HasOne(d => d.Module)
                      .WithMany(p => p.UserPermissions)
                      .HasForeignKey(d => d.ModuleId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("ModuleId_PrmessionModuleId");

            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Archived).HasColumnType("tinyint(3)");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasColumnType("varchar(255)");

                entity.Property(e => e.Key)
                      .IsRequired()
                      .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedUTC)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastUpdatedUTC)
                        .HasColumnType("datetime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Archived)
                      .HasColumnType("tinyint(3)");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Id)
                      .HasDatabaseName("Id_UNIQUE")
                      .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.BusinessUnitId).HasColumnType("int(11)");

                entity.Property(e => e.Archived).HasColumnType("tinyint(3)");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedUTC)
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastUpdatedUTC)
                        .HasColumnType("datetime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Archived)
                      .HasColumnType("tinyint(3)");
            });

            modelBuilder.Entity<UserPermissionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView(nameof(UserPermissionView));

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.ModuleId).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.BusinessUnitId).HasColumnType("int(11)");

                entity.Property(e => e.Title)
                      .HasColumnType("varchar(255)")
                      .UseCollation("utf8_general_ci");

                entity.Property(e => e.RoleName)
                      .HasColumnType("varchar(255)")
                      .UseCollation("utf8_general_ci");

                entity.Property(e => e.Code)
                      .HasColumnType("varchar(255)")
                      .UseCollation("utf8_general_ci");

                entity.Property(e => e.ModuleKey)
                      .HasColumnType("varchar(255)")
                      .UseCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
