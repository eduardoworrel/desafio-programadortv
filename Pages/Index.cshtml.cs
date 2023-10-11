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
    public IActionResult OnPost()
    {

        if (Files == null || Files.Length == 0)
            return BadRequest("file not selected");
        

        var result = new {
            FileName = Path.GetFileNameWithoutExtension(Files.FileName),
            Extension = Path.GetExtension(Files.FileName),
            FileSize = (Files.Length / 1024) + " KB",
            ContentType = Files.ContentType,
        };

        
        return new JsonResult(result);
    }
}
