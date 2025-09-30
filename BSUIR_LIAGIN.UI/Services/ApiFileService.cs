public class ApiFileService : IFileService
{
    private readonly HttpClient _httpClient;

    public ApiFileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SaveFileAsync(IFormFile formFile)
    {
        var extension = Path.GetExtension(formFile.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(formFile.OpenReadStream());
        content.Add(streamContent, "file", newName);

        var response = await _httpClient.PostAsync("", content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        return string.Empty;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        await _httpClient.DeleteAsync(fileName);
    }
}
