using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace desafio.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public IFormFile Files {get; set;}
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostAsync()
    {

        if (Files == null || Files.Length == 0)
            return BadRequest("file not selected");



        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffprobe",
                Arguments = "-v error -hide_banner -print_format json -show_format -show_streams pipe:0",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            }
        };

        process.Start();

        Task.Run(() =>
        {
           while(!process.StandardError.EndOfStream)
           {
               var line = process.StandardError.ReadLine();
               _logger.LogInformation(line);
           }
        });

        await Files.CopyToAsync(process.StandardInput.BaseStream);

        process.StandardInput.Close();

        process.WaitForExit();
        
        var json = process.StandardOutput.ReadToEnd();
        

        var result = new {
            FileName = Path.GetFileNameWithoutExtension(Files.FileName),
            Extension = Path.GetExtension(Files.FileName),
            FileSize = (Files.Length / (1024 * 1024)) + " MB",
            ContentType = Files.ContentType,
            ffmpegData = json
        };
  
        return new JsonResult(result);
    }
}
