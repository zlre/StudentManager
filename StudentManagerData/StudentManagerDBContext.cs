namespace StudentManagerData
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StudentManagerDBContext : DbContext
    {
        public StudentManagerDBContext(DbContextOptions<StudentManagerDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStudent> UserStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Id)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .Property(s => s.Id)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Sex)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(s => s.Surname)
                .HasMaxLength(40)
                .IsRequired();            
            
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .HasMaxLength(40)
                .IsRequired();            
            
            modelBuilder.Entity<Student>()
                .Property(s => s.Patronymic)
                .HasMaxLength(60)
                .IsRequired();
                        
            
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentID)
                .HasMaxLength(16);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentID)
                .IsUnique();


            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Login)
                .HasMaxLength(60)
                .IsRequired();            
            
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(60)
                .IsRequired();
            

            modelBuilder.Entity<UserStudent>()
                .HasKey(us => new { us.UserId, us.StudentId });

            modelBuilder.Entity<UserStudent>()
                .HasOne(us => us.User)
                .WithMany(us => us.UserStudents)
                .HasForeignKey(us => us.UserId);            
            
            modelBuilder.Entity<UserStudent>()
                .HasOne(us => us.Student)
                .WithMany(us => us.UserStudents)
                .HasForeignKey(us => us.StudentId);
                
        }
    }
}
