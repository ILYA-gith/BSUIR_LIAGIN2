using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly string _imagePath;

    public FilesController(IWebHostEnvironment env)
    {
        _imagePath = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Images");
        if (!Directory.Exists(_imagePath))
        {
            Directory.CreateDirectory(_imagePath);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не передан");

        var extension = Path.GetExtension(file.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        var filePath = Path.Combine(_imagePath, newName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var host = HttpContext.Request.Host;
        var scheme = HttpContext.Request.Scheme;
        var fileUrl = $"{scheme}://{host}/Images/{newName}";

        return Ok(fileUrl);
    }

    [HttpDelete("{fileName}")]
    public IActionResult DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return BadRequest("Имя файла не указано");

        var justFileName = Path.GetFileName(fileName);
        var filePath = Path.Combine(_imagePath, justFileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound("Файл не найден");

        System.IO.File.Delete(filePath);
        return Ok();
    }
}
