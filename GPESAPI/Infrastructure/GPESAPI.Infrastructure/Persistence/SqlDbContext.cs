using GPESAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Professor
            modelBuilder.Entity<Professor>()
                .HasKey(p => p.ProfessorId);

            // User
            modelBuilder.Entity<User>()
                .HasKey(c => c.UserId);

            // Project
            modelBuilder.Entity<Project>()
                .HasKey(c => c.ProjectId);

            // Team
            modelBuilder.Entity<Team>()
                .HasKey(t => t.TeamId);

            modelBuilder.Entity<TeamMember>()
                .HasNoKey();

            // Evaluation
            modelBuilder.Entity<Evaluation>()
                .HasKey(e => e.EvaluationId);

            // Report
            modelBuilder.Entity<Report>()
                .HasKey(r => r.ReportId);

            // ProfessorAvailability
            modelBuilder.Entity<ProfessorAvailability>()
                .HasKey(a => a.AvailabilityId);

            // ProfessorsUsers - eklenen ilişki
            modelBuilder.Entity<ProfessorsUsers>()
                .HasKey(pu => new { pu.ProfessorId, pu.UserId });

            //ChecklistItem
            modelBuilder.Entity<ChecklistItem>()
                .HasKey(ci => ci.ItemId);

            modelBuilder.Entity<ChecklistItemDetail>()
                .HasNoKey();

            modelBuilder.Entity<EvaluationCriteriaDetail>()
                .HasNoKey();

            //EvaluationCriteria
            modelBuilder.Entity<EvaluationCriteria>()
                .HasKey(ci => ci.CriteriaId);

            // İlişkiler
            modelBuilder.Entity<ProfessorsUsers>()
                .HasOne<Professor>()
                .WithMany()
                .HasForeignKey(pu => pu.ProfessorId);

            modelBuilder.Entity<ProfessorsUsers>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            modelBuilder.Entity<TeamPresentation>()
                .ToTable("TeamPresentations");

            modelBuilder.Entity<ChecklistItem>()
                    .Property(c => c.ItemId)
                    .ValueGeneratedOnAdd();

            modelBuilder.Entity<Evaluation>()
                    .Property(c => c.EvaluationId)
                    .ValueGeneratedOnAdd();

            modelBuilder.Entity<EvaluationCriteria>()
                    .Property(c => c.CriteriaId)
                    .ValueGeneratedOnAdd();

            // Evaluation ve EvaluationCriteriaDetail arasındaki ilişkiyi belirtmek
            modelBuilder.Entity<EvaluationCriteriaDetail>()
                .HasKey(e => new { e.EvaluationId, e.CriteriaId });

            // Foreign Key ilişkilerini belirtmek
            modelBuilder.Entity<EvaluationCriteriaDetail>()
                .HasOne<Evaluation>()
                .WithMany() // Evaluation ile ilişki
                .HasForeignKey(e => e.EvaluationId);

            modelBuilder.Entity<EvaluationCriteriaDetail>()
                .HasOne<EvaluationCriteria>()
                .WithMany() // EvaluationCriteria ile ilişki
                .HasForeignKey(e => e.CriteriaId);

            // ChecklistItemDetail ile ChecklistItem ve Evaluation arasındaki ilişkiyi belirtmek
            modelBuilder.Entity<ChecklistItemDetail>()
                .HasKey(ci => new { ci.EvaluationId, ci.ItemId }); // Kompozit anahtar oluşturuluyor

            // Foreign Key ilişkilerini belirtmek
            modelBuilder.Entity<ChecklistItemDetail>()
                .HasOne<Evaluation>() // Evaluation ile ilişki
                .WithMany() // Evaluation ile birden çok ilişki olabilir
                .HasForeignKey(ci => ci.EvaluationId);

            modelBuilder.Entity<ChecklistItemDetail>()
                .HasOne<ChecklistItem>() // ChecklistItem ile ilişki
                .WithMany() // ChecklistItem ile birden çok ilişki olabilir
                .HasForeignKey(ci => ci.ItemId);
        
        }

        // DbSet Properties for each table
        public DbSet<User> Users { get; set; }
        public DbSet<ProfessorsUsers> ProfessorsUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<ProfessorAvailability> ProfessorAvailability { get; set; }
        public DbSet<TeamPresentation> TeamPresentations { get; set; }
        public DbSet<EvaluationCriteria> EvaluationCriterias { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<EvaluationCriteriaDetail> EvaluationCriteriaDetails { get; set; }
        public DbSet<ChecklistItemDetail> ChecklistItemDetails { get; set; }
    }
}
