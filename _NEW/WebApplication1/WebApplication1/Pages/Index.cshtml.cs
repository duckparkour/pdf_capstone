﻿using Microsoft.AspNetCore.Hosting;
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
using iTextSharp;
using iTextSharp.text;
using Document = iTextSharp.text.Document;
using Paragraph = iTextSharp.text.Paragraph;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages
{

    public class IndexModel : PageModel
    {
        private readonly DatabaseFileContext db1; // Links the File Storage Database
        public IndexModel(DatabaseFileContext db1) => this.db1 = db1;
        public IFormFile formFile { get; set; } // stores a file uploaded by the user to save to the database.
        public IFormFile audioFormFile { get; set; } //Stores a file created by the user containing a voice recording
        public List<DatabaseFile> FileDatabase { get; set; } = new List<DatabaseFile>(); //List containing the files from the database

        /* 
         Method to allow the user to select a Font type and stores it in the userFont variable.
         */
        public void OnPostChangeFontType(string fontType)
        {
            //Write users font preferences from a file.
            String pathout = ("./Data/USERFONTFILE");
            FileStream stream = new FileStream(pathout, FileMode.OpenOrCreate);
            string[] userData = { "Times", "15", "Gray" };
            StreamReader fileReader = new StreamReader(stream);

            while (!fileReader.EndOfStream)
            {
                userData[0] = fileReader.ReadLine();
                userData[1] = fileReader.ReadLine();
                userData[2] = fileReader.ReadLine();
            }
            stream.Close();
            fileReader.Close();
            
            //Determine which font the user has selected.
            if (fontType == "Helvetica")
            {
                userData[0] = "Helvetica";
            }
            else if (fontType == "TimesRoman")
            {
                userData[0] = "TimesRoman";
            }
            else if (fontType == "Courier")
            {
                userData[0] = "Courier";
            }

            //Write user's selection to file.
            using (StreamWriter outputFile = new StreamWriter(pathout))
            {
                foreach (string line in userData)
                    outputFile.WriteLine(line);
            }
        }


        /* 
        Method to allow the user to select a Font size and stores it in the userFont variable.
        */
        public void OnPostChangeFontSize(string fontSize)
        {
            //Reader user font selections from file.
            String pathout = ("./Data/USERFONTFILE");
            FileStream stream = new FileStream(pathout, FileMode.OpenOrCreate);
            string[] userData = { "Times", "15", "Gray" };
            StreamReader fileReader = new StreamReader(stream);

            while (!fileReader.EndOfStream)
            {
                userData[0] = fileReader.ReadLine();
                userData[1] = fileReader.ReadLine();
                userData[2] = fileReader.ReadLine();
            }
            stream.Close();
            fileReader.Close();

            //Allow user to change font size.
            int convertedFontSize = Convert.ToInt32(fontSize);
            userData[1] = convertedFontSize.ToString();

            //Write changes to file.
            using (StreamWriter outputFile = new StreamWriter(pathout))
            {
                foreach (string line in userData)
                    outputFile.WriteLine(line);
            }
        }

        /* 
        Method to allow the user to select a Font color and stores it in the userFont variable.
        */
        public void OnPostChangeFontColor(string userColor)
        {
            //Reads user font from font file.
            String pathout = ("./Data/USERFONTFILE");
            FileStream stream = new FileStream(pathout, FileMode.OpenOrCreate);
            string[] userData = {"Times","15","Gray" };
            StreamReader fileReader = new StreamReader(stream);

            while (!fileReader.EndOfStream)
            {
               userData[0] = fileReader.ReadLine();
               userData[1] = fileReader.ReadLine();
               userData[2] = fileReader.ReadLine();
            }
            stream.Close();
            fileReader.Close();

            //Allow user to select the font color.
            if (userColor == "Gray")
            {
                userData[2] = "Gray";
            }
            else if (userColor == "Black")
            {
                userData[2] = "Black";

            }
            else if (userColor == "Blue")
            {
                userData[2] = "Blue";

            }
            else if (userColor == "Purple")
            {
                userData[2] = "Purple";

            }
            else if (userColor == "Green")
            {
                userData[2] = "Green";

            }
            else if (userColor == "Yellow")
            {
                userData[2] = "Yellow";

            }
            else if (userColor == "Orange")
            {
                userData[2] = "Orange";

            }
            else if (userColor == "Red")
            {
                userData[2] = "Red";

            }

            //Write font color to file.
            using (StreamWriter outputFile = new StreamWriter(pathout))
            {
                foreach (string line in userData)
                    outputFile.WriteLine(line);
            }
            
        }

        //Load and refresh the database when the page is loaded.
        public async Task OnGetAsync()
        {
            FileDatabase = await db1.Files.ToListAsync();
            await db1.SaveChangesAsync();
        }

        //Ensure that the List of files has been populated from the database. Call in each GET or POST method.
        public void PopulateList()
        {
            foreach (var v in db1.Files)
            {
                FileDatabase.Add(v);
            }
        }

        /*
         Called when user attempts to download a file, requires a fileID to be entered.
         */
        public FileResult OnGetDownloadFile(int fileID)
        {
            PopulateList();
            DatabaseFile downloadableFile = new DatabaseFile();

            //Find the file requested by the user.
            foreach (var f in FileDatabase)
            {
                if (f.FileID == fileID)
                {
                    downloadableFile = f;
                }
            }
            //Read the File data into Byte Array.
            byte[] bytes = downloadableFile.FileContent; //System.IO.File.ReadAllBytes();
            //Send the File to Download.
            return File(bytes, downloadableFile.ContentType, downloadableFile.FileName);
        }

        /*Called when user attempts to add a comment to an existing PDF.
         Requires the text that the user wants to enter, the ID of the file they want it added to, and the page number where the comment should be added.
         */
        public void OnPostAddUserComment(String comment, int fileID, string pagenum, String userFontType, float userFontSize, String userFontColor)
        {
            PopulateList();
            DatabaseFile userFile = new DatabaseFile();

            //Find the file requested by the user.
            foreach (var f in FileDatabase)
            {
                if (f.FileID == fileID)
                {
                    userFile = f;
                }
            }
            Font userFont = new Font(0, 18, 0, BaseColor.BLACK); //Contains the Font selected by the user for writing comments.

            if (userFontType == "Helvetica")
            {
                userFont = FontFactory.GetFont("Helvetica", userFont.Size, userFont.Color);
            }
            else if (userFontType == "TimesRoman")
            {
                userFont = FontFactory.GetFont("TimesRoman", userFont.Size, userFont.Color);
            }
            else if (userFontType == "Courier")
            {
                userFont = FontFactory.GetFont("Courier", userFont.Size, userFont.Color);
            }

            userFont = FontFactory.GetFont(userFont.Family.ToString(), userFontSize, userFont.Color);

            if (userFontColor == "Gray")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.GRAY);
            }
            else if (userFontColor == "Black")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.BLACK);

            }
            else if (userFontColor == "Blue")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.BLUE);

            }
            else if (userFontColor == "Purple")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.
                    MAGENTA);

            }
            else if (userFontColor == "Green")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.GREEN);

            }
            else if (userFontColor == "Yellow")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.YELLOW);

            }
            else if (userFontColor == "Orange")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.ORANGE);

            }
            else if (userFontColor == "Red")
            {
                userFont = FontFactory.GetFont(userFont.Family.ToString(), userFont.Size, BaseColor.RED);

            }


            //Create a temporary file path to store the file.
            String pathout = ("./Data/NEWFILE");
            //Create a PDF reader object to read the content of the file the user wants to add.
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(userFile.FileContent);
            //String to store the page numbers.
            String tempstring = reader.NumberOfPages.ToString();
            FileStream stream = new FileStream(pathout, FileMode.Create);

            //select the original document.
            reader.SelectPages("1-" + tempstring);
            //create PdfStamper object to write to get the pages from reader.
            PdfStamper stamper = new PdfStamper(reader, stream);
            // PdfContentByte from stamper to add content to the pages over the original content.
            PdfContentByte pbover = stamper.GetOverContent(Int32.Parse(pagenum));

            //add content to the page using ColumnText
            ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(comment, userFont), 100, 500, 0);
            stamper.Close();
            //Create a new File to hold the PDF with added comments
            FileStream tempFile = new FileStream(pathout, FileMode.Open);

            //Read original file data.
            BinaryReader br = new BinaryReader(tempFile);
            byte[] buffer = br.ReadBytes((int)tempFile.Length);

            //Add data from original file to temporary file.
            DatabaseFile uploadedFile = new DatabaseFile();
            uploadedFile.FileName = userFile.FileName;
            uploadedFile.ContentType = userFile.ContentType;
            uploadedFile.FileID = userFile.FileID;
            uploadedFile.FileSize = (int)tempFile.Length;
            uploadedFile.FileExtension = userFile.FileExtension;
            uploadedFile.FileContent = buffer;

            //Delete original file and add changed file.
            db1.Remove(userFile);
            db1.Add(uploadedFile);
            db1.SaveChanges();

            //Close resources
            reader.Close();
            tempFile.Close();
        }

        //Allows the user to split a PDF into a smaller segment and save it as a separate file.
        public void OnPostSplitPDF(int startPage, int endPage, int fileID)
        {
            PopulateList();
            DatabaseFile downloadableFile = new DatabaseFile();
            String pathout = ("./Data/SPLITFILE");
            //Find file requested by the user.
            foreach (var f in FileDatabase)
            {
                if (f.FileID == fileID)
                {
                    downloadableFile = f;
                }
            }

            //Create new pdfreader object to get file input from the user.
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(downloadableFile.FileContent);
            //Create stringbuilder to hold the text from each page.
            StringBuilder text = new StringBuilder();
            reader.SelectPages(startPage.ToString() + "-" + endPage.ToString());

            FileStream stream = new FileStream(pathout, FileMode.Create);
            PdfStamper stamper = new PdfStamper(reader, stream);
            stamper.Close();

            //Create new Randomizer.
            Random randomizer = new Random();
            //Encode file contents into byte array.
            //Read file into new filestream
            DatabaseFile uploadedFile = new DatabaseFile();

            //Create a new File to hold the PDF with added comments
            FileStream tempFile = new FileStream(pathout, FileMode.Open);

            //Read original file data.
            BinaryReader br = new BinaryReader(tempFile);
            byte[] buffer = br.ReadBytes((int)tempFile.Length);

            //Add data to the new File.
            uploadedFile.FileName = "Copy " + downloadableFile.FileName;
            uploadedFile.ContentType = downloadableFile.ContentType;
            uploadedFile.FileID = randomizer.Next();
            uploadedFile.FileSize = buffer.Length;
            uploadedFile.FileExtension = ".pdf";
            uploadedFile.FileContent = buffer;

            //Add file and save changes.
            db1.Add(uploadedFile);
            db1.SaveChanges();

            tempFile.Close();
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
            //Create integer randomizer.
            Random randomizer = new Random();

            //Use Binary reader to read file and convert to byte array.
            BinaryReader br = new BinaryReader(formFile.OpenReadStream());
            byte[] buffer = br.ReadBytes((int)formFile.Length);

            //Create a database file class and add data.
            DatabaseFile uploadedFile = new DatabaseFile();
            uploadedFile.FileName = formFile.FileName;
            uploadedFile.ContentType = formFile.ContentType;
            uploadedFile.FileID = randomizer.Next();
            uploadedFile.FileSize = (int)formFile.Length;
            uploadedFile.FileExtension = System.IO.Path.GetExtension(formFile.FileName);
            uploadedFile.FileContent = buffer;

            //Add file and save.
            db1.Add(uploadedFile);
            db1.SaveChanges();

        }

        public void SaveFile(StringBuilder fileContents, DatabaseFile formFile)
        {

            Random randomizer = new Random();
            DatabaseFile uploadedFile = new DatabaseFile();
            uploadedFile.FileName = formFile.FileName;
            uploadedFile.ContentType = formFile.ContentType;
            uploadedFile.FileID = randomizer.Next();
            uploadedFile.FileExtension = ".pdf";
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

        public void LinkFilesToPDF(int FileID)
        {

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
            var writer = new iText.Kernel.Pdf.PdfWriter(stream);
            var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var document = new Document();
            document.Add(new Paragraph(""));
            document.Close();

            return new JsonResult(stream.ToArray());
        }
    }

    
}