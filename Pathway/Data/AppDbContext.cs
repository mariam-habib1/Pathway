using Microsoft.EntityFrameworkCore;
using Pathway.Models;

namespace Pathway.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseSection> CourseSections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
               .HasKey(u => u.UserId);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseId);

            modelBuilder.Entity<CourseSection>()
                .HasKey(s => s.SectionId);

            modelBuilder.Entity<Lesson>()
                .HasKey(l => l.LessonId);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.EnrollmentId);

            modelBuilder.Entity<Course>()
              .Property(c => c.Price)
              .HasPrecision(10, 2);

            // =========================
            // Unique Constraints
            // =========================

            // Email must be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Category name must be unique
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Student cannot enroll in the same course more than once
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique();


            // =========================
            // User → Courses
            // One Instructor can have many Courses
            // =========================

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);


            // =========================
            // Category → Courses
            // One Category can have many Courses
            // =========================

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);


            // =========================
            // Course → CourseSections
            // One Course can have many Sections
            // =========================

            modelBuilder.Entity<CourseSection>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================
            // CourseSection → Lessons
            // One Section can have many Lessons
            // =========================

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Section)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================
            // User → Enrollments
            // One Student can have many Enrollments
            // =========================

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);


            // =========================
            // Course → Enrollments
            // One Course can have many Enrollments
            // =========================

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Web Development", Description = "Build modern websites and web apps" },
                new Category { CategoryId = 2, Name = "Cyber Security", Description = "Protect systems and data from threats" },
                new Category { CategoryId = 3, Name = "Artificial Intelligence", Description = "Machine learning and AI fundamentals" }
);

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Name = "Ahmed Hassan", Email = "ahmed.instructor@pathway.com", PasswordHash = "TEMP_HASH_1", Role = "Instructor", CreatedAt = new DateTime(2026, 1, 1) },
                new User { UserId = 2, Name = "Salma Adel", Email = "salma.instructor@pathway.com", PasswordHash = "TEMP_HASH_2", Role = "Instructor", CreatedAt = new DateTime(2026, 1, 1) }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, Title = "ASP.NET Core MVC from Scratch", Description = "Learn to build full-stack web applications using ASP.NET Core MVC, Entity Framework, and SQL Server.", Price = 499, InstructorId = 1, CategoryId = 1, CreatedAt = new DateTime(2026, 2, 1) },
                new Course { CourseId = 2, Title = "Ethical Hacking Essentials", Description = "A hands-on introduction to penetration testing, network security, and vulnerability assessment.", Price = 699, InstructorId = 2, CategoryId = 2, CreatedAt = new DateTime(2026, 2, 10) },
                new Course { CourseId = 3, Title = "Machine Learning Foundations", Description = "Understand the core concepts behind supervised and unsupervised learning with real datasets.", Price = 599, InstructorId = 2, CategoryId = 3, CreatedAt = new DateTime(2026, 2, 15) },
                new Course { CourseId = 4, Title = "React for Beginners", Description = "Build interactive user interfaces with React, hooks, and modern JavaScript.", Price = 449, InstructorId = 1, CategoryId = 1, CreatedAt = new DateTime(2026, 2, 20) },
                new Course { CourseId = 5, Title = "Network Security Fundamentals", Description = "Learn firewalls, VPNs, and intrusion detection to secure enterprise networks.", Price = 649, InstructorId = 2, CategoryId = 2, CreatedAt = new DateTime(2026, 2, 25) },
                new Course { CourseId = 6, Title = "Deep Learning with Python", Description = "Dive into neural networks, CNNs, and practical AI projects using PyTorch.", Price = 749, InstructorId = 2, CategoryId = 3, CreatedAt = new DateTime(2026, 3, 1) }
            );
        }
    }
}
    