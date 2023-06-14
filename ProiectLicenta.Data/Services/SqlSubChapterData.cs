using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlSubChapterData : ISubChapterData
    {
        private ApplicationDataDbContext db;

        public SqlSubChapterData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public SubChapter getSubChapter(int subchapterId)
        {
            return db.SubChapterList.Include("Chapter.Subchapters")
                .Include("Chapter.Course")
                .Include("SubchapterFiles")
                .Include("Test")
                .FirstOrDefault(sch=>sch.SubchapterId== subchapterId);
        }

        public void AddSubChapter(SubChapter subChapter)
        {
            var chapter = db.ChapterList.FirstOrDefault(c => c.ChapterId == subChapter.ChapterId);
            if (chapter != null)
            {
                int numberOfSubChapters;
                if(chapter?.Subchapters != null && chapter.Subchapters.Count > 0)
                {
                    numberOfSubChapters = chapter.Subchapters.Max(sch => sch.SubchapterNumber);
                }
                else
                {
                    numberOfSubChapters = 0;
                }
                subChapter.SubchapterNumber = numberOfSubChapters + 1;

                chapter.Subchapters.Add(subChapter);
                db.SaveChanges();
            }
        }

        void ISubChapterData.DeleteSubChapter(int id)
        {
            var currentSubChapter = db.SubChapterList.FirstOrDefault(sbch=>sbch.SubchapterId == id);

            var subchapter = getSubChapter(id);
            var chapterNumber = subchapter.Chapter.Subchapters.FirstOrDefault(ch => ch.SubchapterId == id).SubchapterNumber;
            if (subchapter.Chapter.Subchapters.ToList().Count > 1)
            {
                var maxSubChapter = subchapter.Chapter.Subchapters.Where(ch => ch.SubchapterId != subchapter.SubchapterId).Max(ch => ch.SubchapterNumber);

                if (maxSubChapter > chapterNumber)
                {

                    var subchapters = subchapter.Chapter.Subchapters.Where(ch => ch.SubchapterNumber > subchapter.SubchapterNumber).ToList();
                    foreach (var ch in subchapters)
                    {
                        ch.SubchapterNumber = ch.SubchapterNumber - 1;
                    }
                }
            }

            db.SubChapterList.Remove(currentSubChapter);
            db.SaveChanges();
        }

        SubChapter ISubChapterData.GetSubChapter(int subChapterId)
        {
            var subChapter = db.SubChapterList.Include("Chapter.Course")
                .FirstOrDefault(ch => ch.SubchapterId == subChapterId);
            if (subChapter != null)
            {
                return subChapter;
            }
            else
            {
                // to implement;
                return null;
            }
        }

        List<SubChapter> ISubChapterData.GetSubChapters(int chapterId)
        {
            var model = db.ChapterList.FirstOrDefault(ch => ch.ChapterId == chapterId);
            return model.Subchapters.OrderBy(sch => sch.SubchapterNumber).ToList() == null ? null : model.Subchapters.OrderBy(sch => sch.SubchapterNumber).ToList();

        }

        void ISubChapterData.UpdateSubChapter(SubChapter SubChapter)
        {
            var oldSubchapter = getSubChapter(SubChapter.SubchapterId);

            if(oldSubchapter.SubchapterNumber != SubChapter.SubchapterNumber)
            {
                var numberOfSubchapters = oldSubchapter.Chapter.Subchapters.Max(sch => sch.SubchapterNumber);
                if(numberOfSubchapters <= SubChapter.SubchapterNumber)
                {
                    SubChapter.SubchapterNumber = numberOfSubchapters + 1;
                }
                else
                {
                    if(SubChapter.SubchapterNumber > oldSubchapter.SubchapterNumber)
                    {
                        SubChapter.SubchapterNumber += 1;
                    }
                    var subchapters = oldSubchapter.Chapter.Subchapters.Where(sch=> sch.SubchapterNumber >= SubChapter.SubchapterNumber && sch.SubchapterId != SubChapter.SubchapterId).OrderBy(sch=>sch.SubchapterNumber).ToList();
                    foreach (var sch in subchapters)
                    {
                        sch.SubchapterNumber = sch.SubchapterNumber + 1;
                    }
                }
            }
            db.SubChapterList.AddOrUpdate(SubChapter);
            int firstSubchp = 1;
            SubChapter lastSubChp = null;
            var subChapters = oldSubchapter.Chapter.Subchapters.OrderBy(ch => ch.SubchapterNumber).ToList();
            foreach (var schp in subChapters)
            {
                if (firstSubchp == 1)
                {
                    lastSubChp = schp;
                    if (schp.SubchapterNumber != firstSubchp)
                    {
                        schp.SubchapterNumber = firstSubchp;
                    }
                    firstSubchp = -1;
                }
                else
                {

                    schp.SubchapterNumber = lastSubChp.SubchapterNumber + 1;
                    lastSubChp = schp;
                }


            }
            if (SubChapter != null)
            {
                
                db.SaveChanges();
            }
        }

        // if the chapter is finished, the subchapter of the next course will be returned.
        // null if the course is finished
        public SubChapter GetNextSubChapter(int currentsubChapterId)
        {
            var currentSubchapter = getSubChapter(currentsubChapterId);

            var numberOfSubchapters = currentSubchapter.Chapter.Subchapters.Count();

            if(currentSubchapter.SubchapterNumber == numberOfSubchapters)
            {
                //the user has promoted the chapter!

                var currentChapter = currentSubchapter.Chapter;
                var numberOfChapters = currentChapter.Course.Chapters.Count();

                if(numberOfChapters < currentChapter.ChapterNumber + 1) // the student finished the course
                {
                    return null;
                } 

                var nextChapter = db.ChapterList.FirstOrDefault(
                    ch => ch.CourseId == currentChapter.CourseId &&ch.ChapterNumber == (currentChapter.ChapterNumber + 1));
                var subchapterToReturn = nextChapter.Subchapters.OrderBy(sc => sc.SubchapterNumber).FirstOrDefault();

                return subchapterToReturn;
            }

            //caz clasic
            var nextSubChapterId = db.SubChapterList.
                FirstOrDefault(ch => ch.ChapterId == currentSubchapter.ChapterId && 
                ch.SubchapterNumber == (currentSubchapter.SubchapterNumber + 1)).SubchapterId;

            var subchapter = getSubChapter(nextSubChapterId);
            return subchapter;
        }

    }
}
