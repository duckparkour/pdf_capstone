using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        /*        private readonly ILogger<IndexModel> _logger;
        */
        //private readonly AudioContext db;
        // public IndexModel(AudioContext db) => this.db = db;
        // public List<AudioModel> Audios { get; set; } = new List<AudioModel>();

        private readonly DatabaseFileContext db1;

        public IndexModel(DatabaseFileContext db1) => this.db1 = db1;
        public IFormFile formFile { get; set; }

        // public IHostingEnvironment hostingEnvironment;
        public List<DatabaseFile> FileDatabase { get; set; } = new List<DatabaseFile>();

        // public IndexModel(IHostingEnvironment environment)
        // {
        //     this.hostingEnvironment = environment;
        // }

        public async Task OnGetAsync()
        {
            //Audios = await db.Audios.ToListAsync();
            FileDatabase = await db1.Files.ToListAsync();
            //FileDatabase = await db1.Files.AddAsync();
            //await db1.SaveChangesAsync();
        }

        public void OnPost()
        {

            Random randomizer = new Random();
            BinaryReader br = new BinaryReader(formFile.OpenReadStream());
            byte[] buffer = br.ReadBytes((int)formFile.Length);
            DatabaseFile uploadedFile = new DatabaseFile();
            uploadedFile.FileName = formFile.FileName;
            uploadedFile.ContentType = formFile.ContentType;
            uploadedFile.FileID = randomizer.Next();
            uploadedFile.FileSize = (int)formFile.Length;
            uploadedFile.FileExtension = Path.GetExtension(formFile.FileName);
            uploadedFile.FileContent = buffer;
            db1.Add(uploadedFile);
            db1.SaveChanges();

        }

        public ContentResult OnGetFile(int ID)
        {
            ContentResult fileToGet = new ContentResult();
            foreach (DatabaseFile file in db1.Files)
            {
                if (file.FileID == ID)
                {
                    fileToGet.Content = file.FileName;
                }
            }
            
            return fileToGet;
        }

        public void FileDownload()
        {

        }

        public void FileUpload()
        {

        }


    }

}
