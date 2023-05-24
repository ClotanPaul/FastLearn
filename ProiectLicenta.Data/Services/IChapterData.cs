using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface IChapterData
    {

        List<Chapter> getCourseChapters(int courseId);
        void AddChapter(Chapter chapter);

        Chapter GetChapter(int id);

        void UpdateChapter(Chapter chapter);

        void DeleteChapter(int id);
    }
}
