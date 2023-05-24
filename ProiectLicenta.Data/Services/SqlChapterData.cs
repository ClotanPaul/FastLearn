using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ProiectLicenta.Data.Services
{
    public class SqlChapterData : IChapterData
    {

        private ApplicationDataDbContext db;

        public SqlChapterData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public Chapter GetChapter(int chapterId)
        {
            var model = db.ChapterList.FirstOrDefault(ch=>ch.ChapterId== chapterId);
            return model == null ? null : model;
        }

        public List<Chapter> getCourseChapters(int courseId)
        { 
            var courseChapters = new List<Chapter>();

            courseChapters = db.ChapterList.Where(ch => ch.CourseId == courseId)
                                            .OrderBy(ch =>ch.ChapterNumber)
                                            .ToList();

            return courseChapters;
        }

        public void UpdateChapter(Chapter chapter)
        {
            //see if the chapter number was affected
            var oldchapter = GetChapter(chapter.ChapterId);

            if (oldchapter.ChapterNumber != chapter.ChapterNumber)
            {
                // ce e inainte ramane la fel
                // ce e dupa creste cu unu.
                //courseNr>=
                var course = db.Courses.FirstOrDefault(c => c.CourseId == chapter.CourseId);
                var numberOfChapters = course.Chapters.Where(ch => ch.ChapterId != chapter.ChapterId).Max(ch => ch.ChapterNumber );
                if (numberOfChapters <= chapter.ChapterNumber)
                {
                    chapter.ChapterNumber = numberOfChapters + 1;
                }
                else
                {
                    var chapterList = new List<Chapter>();
                    if(chapter.ChapterNumber > oldchapter.ChapterNumber) 
                    {
                        chapter.ChapterNumber += 1;
                    }
                    var chaptersList = course.Chapters.Where(ch => ch.ChapterNumber >= chapter.ChapterNumber && ch.ChapterId != chapter.ChapterId).ToList();
                    // mergea cu >= pe restu la cazuri
                    foreach (var ch in chaptersList)
                    {
                        ch.ChapterNumber = ch.ChapterNumber + 1;
                    }

                }
            }
            //chapter.ChapterNumber += 1; // 1->3
            db.ChapterList.AddOrUpdate(chapter);
            //3->1 - ca inainte

            // make sure the chapter numbers have no gap and start from 1
            int firstChp = 1;
            Chapter lastChp = null;
            var chapters = oldchapter.Course.Chapters.OrderBy(ch => ch.ChapterNumber).ToList();
            foreach (var chp in chapters)
            {
                if(firstChp == 1)
                {
                    lastChp = chp;
                    if (chp.ChapterNumber != firstChp)
                    {
                        chp.ChapterNumber = firstChp;
                    }
                    firstChp = -1;
                }
                else
                {
                    
                    chp.ChapterNumber = lastChp.ChapterNumber + 1;
                    lastChp = chp;
                }


            }
            if (chapter != null)
            {
                
                db.SaveChanges();
            }
        }

        void IChapterData.AddChapter(Chapter chapter)
        {
            var course = db.Courses.FirstOrDefault(c => c.CourseId == chapter.CourseId);
            if (course != null)
            {
                int numberOfChapters;
                if (course?.Chapters != null && course.Chapters.Count > 0)
                {
                    numberOfChapters = course.Chapters.Max(ch => ch.ChapterNumber);
                }
                else
                {
                    numberOfChapters = 0;
                }
                chapter.ChapterNumber= numberOfChapters + 1;

                course.Chapters.Add(chapter);
                //db.ChapterList.Add(chapter);
                db.SaveChanges();
            }
               
        }

        public void DeleteChapter(int chapterId)
        {
            var currentChapter = db.ChapterList.FirstOrDefault(x => x.ChapterId == chapterId);
            var chapter = GetChapter(chapterId);
            var chapterNumber = chapter.Course.Chapters.FirstOrDefault(ch => ch.ChapterId == chapterId).ChapterNumber;
            var maxChapter =  chapter.Course.Chapters.Where(ch => ch.ChapterId != chapter.ChapterId).Max(ch => ch.ChapterNumber);
            if(maxChapter > chapterNumber)
            {
                
                var chapters = chapter.Course.Chapters.Where(ch => ch.ChapterNumber > chapter.ChapterNumber).ToList();
                foreach (var ch in chapters)
                {
                    ch.ChapterNumber = ch.ChapterNumber - 1;
                }
            }
            db.ChapterList.Remove(currentChapter);
            db.SaveChanges();
        }
    }
}
