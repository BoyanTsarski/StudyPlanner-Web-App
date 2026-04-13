using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyPlanner.Core.Models;

namespace StudyPlanner.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudyTask> Tasks { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<SubTask> SubTasks { get; set; } = null!;
    }
}