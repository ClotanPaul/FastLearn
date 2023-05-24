using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlTestData : ITestData
    {
        private ApplicationDataDbContext db;

        public SqlTestData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public void AddTest(Test test, int subChapterId)
        {
            var subChapter = db.SubChapterList.FirstOrDefault(sch=>sch.SubchapterId ==subChapterId);
            if(subChapter != null)
            {
                subChapter.Test = test;
                db.SaveChanges();
            }
        }



        public Test getTest(int subChapterId)
        {
            var subChapter = db.SubChapterList.Include("Test.SubChapter.Chapter.Course")
                .FirstOrDefault(sch => sch.SubchapterId == subChapterId);
                

            if (subChapter?.Test != null)
            {
                return subChapter.Test;
            }
            else
                return null;
        }

        public void UpdateTest(Test test, int subChapterId)
        {
            var subChapter = db.SubChapterList.FirstOrDefault(sch => sch.SubchapterId == subChapterId);
            if(subChapter != null )
            {
                db.Tests.AddOrUpdate(subChapter.Test, test);
                db.SaveChanges();
            }
        }

        public void DeleteTest(Test test)
        {
            if(test != null)
            {
                db.Tests.Remove(test);
                db.SaveChanges();
            }
        }

        public Test getTestById(int testId)
        {
            var test = db.Tests.FirstOrDefault(t => t.TestId == testId);
            if (test != null)
                return test;
            else return null;
        }

    }
}
