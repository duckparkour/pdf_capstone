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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Web;



namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        /*        private readonly ILogger<IndexModel> _logger;
        */
        //private readonly AudioContext db;
        // public IndexModel(AudioContext db) => this.db = db;
        // public List<AudioModel> Audios { get; set; } = new List<AudioModel>();
        /*		public Font userFont = FontFactory.GetFont("Arial", 12, Color.BLACK);
        */
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

        /*public Enum Colors
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
		*/
        /*
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
		*/
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

        public void OnPostAudio(string fileName)
        {
            Task<string> audioFileToBeSavedAsBase64;
            var reader = new StreamReader(Request.Body);
            audioFileToBeSavedAsBase64 = reader.ReadToEndAsync();

            byte[] buffer = System.Convert.FromBase64String(audioFileToBeSavedAsBase64.Result);
            Random randomizer = new Random();
            DatabaseFile audioToBeSaved = new DatabaseFile();
            audioToBeSaved.FileName = fileName;
            audioToBeSaved.ContentType = "audio/mp4";
            audioToBeSaved.FileID = randomizer.Next();
            audioToBeSaved.FileExtension = ".mp4";
            audioToBeSaved.FileContent = buffer;
            audioToBeSaved.FileSize = buffer.Length;
            db1.Add(audioToBeSaved);
            db1.SaveChanges();

            
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

        //This method post the current file in the iFrame to the database when it is saved.
        public void OnPostNewPdf(string fileName)
        {
            Task<string> pdfToBeSavedAsBase64;
            var reader = new StreamReader(Request.Body);
            pdfToBeSavedAsBase64 = reader.ReadToEndAsync();

            byte[] buffer = System.Convert.FromBase64String(pdfToBeSavedAsBase64.Result);
            Random randomizer = new Random();
            DatabaseFile pdfToBeSaved = new DatabaseFile();
            pdfToBeSaved.FileName = fileName;
            pdfToBeSaved.ContentType = "application/pdf";
            pdfToBeSaved.FileID = randomizer.Next();
            pdfToBeSaved.FileExtension = ".pdf";
            pdfToBeSaved.FileContent = buffer;
            pdfToBeSaved.FileSize = buffer.Length;
            db1.Add(pdfToBeSaved);
            db1.SaveChanges();

        }


        public JsonResult OnGetANewPdfFile(string fileName)
        {
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            document.Add(new Paragraph(""));
            document.Close();

            return new JsonResult(stream.ToArray());
        }
    }
}
