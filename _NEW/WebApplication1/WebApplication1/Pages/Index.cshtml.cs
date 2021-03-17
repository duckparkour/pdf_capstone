using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        /*        private readonly ILogger<IndexModel> _logger;
        */
        private readonly AudioContext db;
        public IndexModel(AudioContext db) => this.db = db;
        public List<AudioModel> Audios { get; set; } = new List<AudioModel>();

        public async Task OnGetAsync()
        {
            Audios = await db.Audios.ToListAsync();
        }

        public void FileDownload()
        { 
        
        }

        public void FileUpload()
        { 
        
        }
        /*  public IndexModel(ILogger<IndexModel> logger)
          {
              _logger = logger;
          }*/


    }
}
