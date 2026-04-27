using Microsoft.EntityFrameworkCore;
using Neptun.Models;

namespace Neptun.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<CourseModel> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubjectModel>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CourseModel>().Property(c => c.Id).ValueGeneratedOnAdd();

           modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<CourseModel>()
                .HasMany(c => c.Teachers)
                .WithMany()
                .UsingEntity(j => j.ToTable("course_teachers")); 
          
            modelBuilder.Entity<CourseModel>()
                .HasMany(c => c.Students)
                .WithMany()
                .UsingEntity(j => j.ToTable("course_students")); 
        }
    }
}