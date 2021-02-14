using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        //Load the webpage
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateUploadedFiles();
            }
        }

        //Populate the list of files available for download by pulling from the database.
        private void PopulateUploadedFiles()
        {
            //Uses the entities from the database to populate table.
            using (Database1Entities dc = new Database1Entities())
            {
                List<Table> allFiles = dc.Tables.ToList(); //Creates list of files.
                DataList1.DataSource = allFiles;
                DataList1.DataBind();
            }
        }

        //Function to upload a file to the database.
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Code to upload the files to the database.
            if (FileUpload1.HasFile)
            {
                HttpPostedFile file = FileUpload1.PostedFile;
                BinaryReader br = new BinaryReader(file.InputStream);
                byte[] buffer = br.ReadBytes(file.ContentLength);

                using (Database1Entities dc = new Database1Entities())
                {
                    //Create the table object to store the files in the database.
                    dc.Tables.Add(
                        new Table
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            FileID = 0,
                            FileSize = file.ContentLength,
                            FileExtension = Path.GetExtension(file.FileName),
                            FileContent = buffer
                        });
                    dc.SaveChanges();
                    PopulateUploadedFiles();
                }
            }
        }

        //Function to download a file from the database.
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs ec)
        {
                if (ec.CommandName == "Download")
                {
                    int fileID = Convert.ToInt32(ec.CommandArgument);
                    using (Database1Entities dc = new Database1Entities())
                    {
                        var v = dc.Tables.Where(a => a.FileID.Equals(fileID)).FirstOrDefault();
                        if (v != null)
                        {
                            byte[] fileData = v.FileContent;
                            Response.AddHeader("Content-type", v.ContentType);
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + v.FileName);

                            byte[] dataBlock = new byte[0x1000];
                            long fileSize;
                            int bytesRead;
                            long totalBytesRead = 0;

                            //Use a stream object to download the file to a client machine.
                            using (Stream st = new MemoryStream(fileData))
                            {
                                fileSize = st.Length;
                                while (totalBytesRead < fileSize)
                                {
                                    if (Response.IsClientConnected)
                                    {
                                        bytesRead = st.Read(dataBlock, 0, dataBlock.Length);
                                        Response.OutputStream.Write(dataBlock, 0, bytesRead);

                                        Response.Flush();
                                        totalBytesRead += bytesRead;
                                    }
                                }
                            }
                            Response.End();
                        }
                    }
                }
            }
    }
}