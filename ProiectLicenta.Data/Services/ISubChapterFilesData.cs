using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public interface ISubChapterFileData
    {
        void UploadFile(SubChapterFiles file);

        List<SubChapterFiles> GetSubChapterFiles(int subChapterId);

        SubChapterFiles getSubChapterFile(int subChapterId, string fileName);

        SubChapterFiles getSubChapterFileById(int fileId);

        void DeleteFile(int fileId);
    }
}
