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
using System.Text;
using System.Buffers.Text;
using System.Diagnostics;
using System.IO.Pipelines;
using Aspose.Pdf;

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
        //  <td><a href = "@Url.Page("Index", "DownloadFile", new { downloadableFile = file })">Download</a></td>
        public IndexModel(DatabaseFileContext db1) => this.db1 = db1;
        public IFormFile formFile { get; set; }
        public IFormFile audioFormFile { get; set; }

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

        public FileResult OnGetDownloadFile(DatabaseFile downloadableFile)
        {
            //Build the File Path.
            //string path = Path.Combine(this.FileDatabase.IndexOf(downloadableFile) + downloadableFile.FileName);

            //Read the File data into Byte Array.
            byte[] bytes = downloadableFile.FileContent; //System.IO.File.ReadAllBytes();
            //Send the File to Download.
            return File(bytes, downloadableFile.ContentType, downloadableFile.FileName);
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

        public void OnPostAudio(string fileName, int fileSize, string base64EncodedFile)
        {
            Guid guid = Guid.NewGuid();
            base64EncodedFile = Convert.ToBase64String(guid.ToByteArray());
            // byte[] buffer = Base64.Default.Decode(base64EncodedFile);
            // base64EncodedFile = base64EncodedFile.Split(',')[1];
            // Console.WriteLine(base64EncodedFile);

            // byte[] buffer = Convert.FromBase64String(base64EncodedFile);
            // Random randomizer = new Random();
            // DatabaseFile uploadedFile = new DatabaseFile();

            // uploadedFile.FileName = fileName;
            // uploadedFile.ContentType = "audio/mp4";
            // uploadedFile.FileID = randomizer.Next();
            // uploadedFile.FileSize = fileSize;
            // uploadedFile.FileExtension = ".mp4";
            // uploadedFile.FileContent = buffer;            
            // db1.Add(uploadedFile);
            // db1.SaveChanges();
        }

        public JsonResult OnGetFile(int ID)
        {
            foreach (DatabaseFile file in db1.Files)
            {
                if (file.FileID == ID)
                {
                    byte[] fileBytes = file.FileContent;
                    // return new JsonResult(Convert.ToBase64String(fileBytes));
                    return new JsonResult(fileBytes);
                }
            }

            return null;
        }

        public void FileDownload()
        {

        }

        public void FileUpload()
        {

        }

        public JsonResult OnGetANewPdfFile(string fileName)
        {
            byte[] fileBytes = null;
            Document newPdfDocument = new Document();
            Aspose.Pdf.Page page = newPdfDocument.Pages.Add();
            MemoryStream outStream = new MemoryStream();

            newPdfDocument.Save(outStream, SaveFormat.Pdf);
            fileBytes = outStream.ToArray();

            return new JsonResult(fileBytes);
        }
    }
}
