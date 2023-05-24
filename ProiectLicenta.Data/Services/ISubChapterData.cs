using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface ISubChapterData
    {
        SubChapter GetSubChapter(int subChapterId);
        void AddSubChapter(SubChapter SubChapter);


        List<SubChapter> GetSubChapters(int chapterId);

        void UpdateSubChapter(SubChapter SubChapter);

        void DeleteSubChapter(int id);
    }
}
