using Proiect_Licenta.Models;
using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class ApplicationDataDbContext : DbContext
    {
        public DbSet<UserData> UsersData { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Video> VideoList { get; set; }

        public DbSet<Chapter> ChapterList { get; set; }

        public DbSet<SubChapter> SubChapterList { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<UserAnswer> UserAnswers { get; set; }

        public DbSet<CourseReview> CourseReviews { get; set; }

        public DbSet<Warning> Warnings { get; set; }

        public DbSet<SubChapterFiles> SubChapterFiles { get; set; }

        public DbSet<EnrolledStudentInCourse> EnrolledStudentsInCourses { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Mesages { get; set; }

        public DbSet<HelpingStudentApplication> HelpingStudentApplications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SubChapter>()
                .HasOptional(s => s.Test)  
                .WithRequired(t => t.SubChapter)
                .WillCascadeOnDelete(true);

        }
    }
}
