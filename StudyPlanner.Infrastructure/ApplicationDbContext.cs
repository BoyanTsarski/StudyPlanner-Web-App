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

        public DbSet<StudyTask> Tasks { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
    }
}