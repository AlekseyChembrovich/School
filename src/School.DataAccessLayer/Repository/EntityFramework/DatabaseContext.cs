using Microsoft.EntityFrameworkCore;
using School.DataAccessLayer.Models;

namespace School.DataAccessLayer.Repository.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Position> Position { get; set; }

        public virtual DbSet<RegistrationDocument> RegistrationDocument { get; set; }

        public virtual DbSet<TypeDocument> TypeDocument { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-ALEKSEY\\SQLEXPRESS;Initial Catalog=School;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistrationDocument>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.EmployeeCreatorId).HasColumnName("EmployeeCreatorId");
                entity.Property(e => e.EmployeeApproverId).HasColumnName("EmployeeApproverId");

                entity.HasOne(d => d.EmployeeCreator)
                    .WithMany(p => p.RegistrationDocuments1)
                    .HasForeignKey(d => d.EmployeeCreatorId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_EmployeeCreator_RegistrationDocument");

                entity.HasOne(d => d.EmployeeApprover)
                    .WithMany(p => p.RegistrationDocuments2)
                    .HasForeignKey(d => d.EmployeeApproverId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .HasConstraintName("FK_EmployeeApprover_RegistrationDocument");
            });
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DatabaseContext()
        {

        }
    }
}
