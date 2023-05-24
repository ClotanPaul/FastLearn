using Microsoft.AspNet.Identity;
using Proiect_Licenta.ViewModels;
using ProiectLicenta.Data.Models;
using ProiectLicenta.Data.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proiect_Licenta.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoData videoDb;

        public VideoController(IVideoData videoDb)
        {
            this.videoDb = videoDb;
        }
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }


        [HttpGet]
        public ActionResult UploadVideo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadVideo(HttpPostedFileBase file, Video video)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Videos"), fileName);
                file.SaveAs(path);

                video.FileName = fileName;
                video.UploadedDate = DateTime.Now;

                // Save the video data to the database
                videoDb.UploadVideo(video);
            }
            return RedirectToAction("Index");
        }


        public ActionResult GetFileList()
        {
            //set the path to the file
            string fileFolder = Server.MapPath("~/Content/Videos/");

            // get the name of the files in the folder
            DirectoryInfo dirInfo = new DirectoryInfo(fileFolder);
            FileInfo[] files = dirInfo.GetFiles();

            //initialize FileList
            FileListViewModel viewModel = new FileListViewModel();

            foreach (FileInfo file in files)
            {
                viewModel.Files.Add(file.Name);

            }

           return View(viewModel);
        }

        [HttpGet]
        public ActionResult Download(string fileName)
        {

            if(fileName.IsEmpty())
            {
                return RedirectToAction("NotFound");
            }

            // folder path
            string contentFolderPath = Server.MapPath("~/Content/Videos/");
            string fullPath = Path.Combine(contentFolderPath, fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return RedirectToAction("NotFound");
            }

            // Set the MIME type based on the file extension
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            // Return the file to download
            return File(fullPath, mimeType, fileName);
        }
    }
}