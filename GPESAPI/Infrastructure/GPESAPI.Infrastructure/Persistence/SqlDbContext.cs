using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraduateProjectEvaluationSystemAPI.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-10K0RM5\\SQLEXPRESS;Database=GPES;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=True;TrustServerCertificate=True;");
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

            // TeamMember
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => tm.TeamMemberId);

            // ProjectSubmission
            modelBuilder.Entity<ProjectSubmission>()
                .HasKey(ps => ps.SubmissionId);

            // Evaluation
            modelBuilder.Entity<Evaluation>()
                .HasKey(e => e.EvaluationId);

            // Feedback
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.FeedbackId);

            // Report
            modelBuilder.Entity<Report>()
                .HasKey(r => r.Id);

            // ProfessorAvailability
            modelBuilder.Entity<ProfessorAvailability>()
                .HasKey(a => a.AvailabilityId);

            // Panel
            modelBuilder.Entity<Panel>()
                .HasKey(p => p.Id);

            // TeamPanelAssignment
            modelBuilder.Entity<TeamPanelAssignment>()
                .HasKey(pa => pa.Id);

            // ProfessorsUsers - eklenen ilişki
            modelBuilder.Entity<ProfessorsUsers>()
                .HasKey(pu => new { pu.ProfessorId, pu.UserId });

            // İlişkiler burada tanımlanabilir (örneğin)
            modelBuilder.Entity<ProfessorsUsers>()
                .HasOne<Professor>()
                .WithMany()
                .HasForeignKey(pu => pu.ProfessorId);

            modelBuilder.Entity<ProfessorsUsers>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(pu => pu.UserId);
        }

        // DbSet Properties for each table
        public DbSet<User> Users { get; set; }
        public DbSet<ProfessorsUsers> ProfessorsUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ProjectSubmission> ProjectSubmissions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<ProfessorAvailability> ProfessorAvailability { get; set; }
        public DbSet<Panel> Panels { get; set; }
        public DbSet<TeamPanelAssignment> TeamPanelAssignments { get; set; }
    }
}
