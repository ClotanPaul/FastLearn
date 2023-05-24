using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proiect_Licenta.ViewModels
{
    public class FileListViewModel
    {
        public List<string> Files { get; set; }

        public FileListViewModel()
        {
            this.Files = new List<string>();
        }
    }
}