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

        /**
         * Upload a PDF file on your local machine to our server
         *
         * This method sends a file to our database that is populated by a file chooser input on the front end.
         * A DatabaseFile object is generated based on the uploaded file's data and posted to the database
         *
         * @return <void> 
         **/
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

        /**
         * This method posts a new file to the server
         *
         * This method is called when the corresponding HTTP post method is called from the front end. The response body is read 
         * which is a base64 string representing the file to be posted. The method then converts this base64 string into byte[] 
         * representing the files contents. A DatabaseFile object is generated and posted to the server. Depending on the type of file
         * being passed the content type and file extension will either be set to pdf or audio accordingly.
         * 
         * @param <string> <fileName> <The name of the file which is a url query string param in the ajax method calling this method>
         * @param <string> <typeOfFile> <The type of file being passed. Acceptable values are "pdf" or "audio" >
         * @return <void> 
         **/
        public void OnPostFile(string fileName, string typeOfFile)
        {
            Task<string> audioFileToBeSavedAsBase64;
            var reader = new StreamReader(Request.Body);
            audioFileToBeSavedAsBase64 = reader.ReadToEndAsync();

            byte[] buffer = System.Convert.FromBase64String(audioFileToBeSavedAsBase64.Result);
            Random randomizer = new Random();
            DatabaseFile audioToBeSaved = new DatabaseFile();
            audioToBeSaved.FileName = fileName;
            audioToBeSaved.ContentType = typeOfFile.Equals("pdf") ? "application/pdf" : "audio/mp4";
            audioToBeSaved.FileID = randomizer.Next();
            audioToBeSaved.FileExtension = typeOfFile.Equals("pdf") ? ".pdf" : ".mp4";
            audioToBeSaved.FileContent = buffer;
            audioToBeSaved.FileSize = buffer.Length;
            db1.Add(audioToBeSaved);
            db1.SaveChanges();
        }

        /**
         * Return a file from the database which will be represented as a base64 string.
         *
         * This method is used when we need to recieve a file from our database to be used inside of our User Interface. The ID
         * of the file to recieve is passed as a parameter which is then compared against a list of our DatabaseFile objects. If one is
         * not found null is returned. If one is found then the files contents are stored inside a byte array and sent as a Json object
         * which is represented as a base64 string when it reaches the frontend. 
         *
         * @param <int> <ID> <The ID number of the file in the database>
         * @return <JsonResult> <The files contents are being returned>
         **/
        public JsonResult OnGetFile(int ID)
        {
            foreach (DatabaseFile file in db1.Files)
            {
                if (file.FileID == ID)
                {
                    byte[] fileBytes = file.FileContent;
                    return new JsonResult(fileBytes);
                }
            }

            return null;
        }

        /**
         * Get a new blank PDF file
         *
         * A new PDF file is genereated when this method is called. A memory stream object is created which is used in conjunction with 
         * iText 7's PDF api to read the file's bytes into a stream which is returned as a JsonResult. Will be a base64 on the front end. 
         * 
         * @return <JsonResult> <Return the new pdf's bytes as a base64 string> 
         **/
        public JsonResult OnGetANewPdfFile()
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
