using ProiectLicenta.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectLicenta.Data.Services
{
    public class SqlVideoData : IVideoData 
    {
        private ApplicationDataDbContext db;

        public SqlVideoData(ApplicationDataDbContext db)
        {
            this.db = db;
        }

        public void UploadVideo(Video video)
        {
            db.VideoList.Add(video);
            db.SaveChanges();
        }
    }
}
