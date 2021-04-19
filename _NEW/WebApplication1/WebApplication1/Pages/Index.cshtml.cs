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
		public Font userFont = FontFactory.GetFont("Arial", 12, Color.BLACK);
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
		
		public Enum Colors
		{
			Gray = 0,
			Black = 1,
			Blue = 2,
			Purple = 3,
			Green = 4,
			Yellow = 5,
			Orange = 6,
			Red = 7
		}
		
		public Enum FontType
		{
			Helvetica = 0,
			TimesRoman = 1,
			Courier = 2
		}
		
		public void ChangeFontType(FontType fontType,Phrase p )
		{
			if (fontType == Helvetica)
			{
				userFont = FontFactory.GetFont("Helvetica", userFont.getSize(),userFont.getColor());
			}
			else if (fontType == TimesRoman)
			{
				userFont = FontFactory.GetFont("TimesRoman", userFont.getSize(),userFont.getColor());
			}
			else if (fontType == Courier)
			{
				userFont = FontFactory.GetFont("Courier", userFont.getSize(),userFont.getColor());
			}
			
			p = new Phrase(p, userFont);
		}
		
		public void ChangeFontSize(int fontSize, Phrase p)
		{
			userFont = FontFactory.GetFont(userFont.getFamily(),fontSize,userFont.getColor());
			p = new Phrase(p, userFont);
		}
		
		public void ChangeFontColor(Colors userColor, Phrase p)
		{
			
			if (userColor == Gray)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.GRAY);
			}
			else if (userColor == Black)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.BLACK);
			}
			else if (userColor == Blue)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.BLUE);
			}
			else if (userColor == Purple)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.PURPLE);
			}
			else if (userColor == Green)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.GREEN);
			}
			else if (userColor == Yellow)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.YELLOW);
			}
			else if (userColor == Orange)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.ORANGE);
			}
			else if (userColor == Red)
			{
				userFont = FontFactory.GetFont(userFont.getFamily(), userFont.getSize(), Color.RED);
			}
		
			p = new Phrase(p, userFont);
		}

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
			FileDatabase = db1.Files();
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
