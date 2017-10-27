using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;

namespace DockerWebApp.Pages
{
    public class IndexModel : PageModel
    {

        public string Data;

        public IndexModel(IHostingEnvironment hostingEnvironment)
        {
            Data = hostingEnvironment.WebRootPath;
            return;
            var dir = hostingEnvironment.ContentRootFileProvider.GetDirectoryContents("/");
            foreach (var item in dir)
            {
                Data += item.Name + "\n\r";
            }
        }

        public void OnGet()
        {
            

        }
    }
}
