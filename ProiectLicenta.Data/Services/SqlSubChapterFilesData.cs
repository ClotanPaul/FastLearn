using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ProiectLicenta.Data.Services
{
    public class SqlChapterFileData : ISubChapterFileData

    {
        private readonly ApplicationDataDbContext db;
        private readonly ISubChapterData subchapterDb;
        public SqlChapterFileData(ApplicationDataDbContext db, ISubChapterData subchapterDb)
        {
            this.db = db;
            this.subchapterDb= subchapterDb;
        }

        public void DeleteFile(int fileId)
        {
            var file = db.SubChapterFiles.FirstOrDefault(f => f.SubChapterFilesId == fileId);
            db.SubChapterFiles.Remove(file);
            db.SaveChanges();
        }

        public SubChapterFiles getSubChapterFile(int subChapterId, string fileName)
        {
            var file = db.SubChapterFiles.FirstOrDefault(schf => schf.SubChapterId == subChapterId && schf.Title == fileName);
            return file;
        }

        public SubChapterFiles getSubChapterFileById(int fileId)
        {
            return db.SubChapterFiles.FirstOrDefault(f => f.SubChapterFilesId == fileId);
        }

        public List<SubChapterFiles> GetSubChapterFiles(int subchapterId)
        {
            var subChapter = subchapterDb.GetSubChapter(subchapterId);
            var subChapterFiles = subChapter.SubchapterFiles.ToList();
            return subChapterFiles;
        }

        public void UploadFile(SubChapterFiles file)
        {
            db.SubChapterFiles.Add(file);
            db.SaveChanges();
        }
    }
}
