using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BSUIR_LIAGIN.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileService _fileService;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiProductService> logger,
            IFileService fileService)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _pageSize = configuration.GetSection("ItemsPerPage").Value ?? "3";
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;

            _logger.LogInformation(">>> ApiProductService используется <<<");
        }

        // Список книг с фильтром и пагинацией
        public async Task<ResponseData<ListModel<Book>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            try
            {
                var urlBuilder = new StringBuilder($"{_httpClient.BaseAddress}books");

                if (!string.IsNullOrEmpty(categoryNormalizedName))
                    urlBuilder.Append($"/{categoryNormalizedName}");

                var query = new Dictionary<string, string?>
                {
                    ["pageNo"] = pageNo.ToString(),
                    ["pageSize"] = _pageSize
                };
                urlBuilder.Append(QueryString.Create(query));

                var response = await _httpClient.GetAsync(urlBuilder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Book>>>(_serializerOptions);
                    return data ?? ResponseData<ListModel<Book>>.Error("Пустой ответ от API");
                }

                _logger.LogError($"Ошибка при получении книг: {response.StatusCode}");
                return ResponseData<ListModel<Book>>.Error($"Ошибка: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Исключение при получении книг: {ex.Message}");
                return ResponseData<ListModel<Book>>.Error($"Ошибка: {ex.Message}");
            }
        }

        // Получение книги по Id
        public async Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"books/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
                    return data ?? ResponseData<Book>.Error("Пустой ответ от API");
                }

                return ResponseData<Book>.Error($"Ошибка: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при получении книги: {ex.Message}");
                return ResponseData<Book>.Error($"Ошибка: {ex.Message}");
            }
        }

        // Создание книги с сохранением изображения
        public async Task<ResponseData<Book>> CreateProductAsync(Book product, IFormFile? formFile = null)
        {
            try
            {
                // Заполняем обязательные технические поля
                if (formFile != null)
                {
                    var imageUrl = await _fileService.SaveFileAsync(formFile); // обычно абсолютный URL
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // Сохраняем в БД относительный путь: Images/xxx.ext
                        var relative = new Uri(imageUrl).LocalPath.TrimStart('/');
                        product.Image = relative; // e.g., Images/abc.jpg
                        product.ImageMimeType = formFile.ContentType;
                    }
                }
                else
                {
                    product.Image = "Images/noimage.jpg";
                    product.ImageMimeType = "image/jpeg";
                }

                // Важно: CategoryId должен быть заполнен до вызова API.
                // На странице Create мы выставляем product.CategoryId = выбранной категории.
                // Навигационное свойство Category отправлять не нужно.
                product.Category = null;

                var response = await _httpClient.PostAsJsonAsync("books", product, _serializerOptions);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
                    return data ?? ResponseData<Book>.Error("Пустой ответ от API");
                }

                _logger.LogError($"Ошибка при создании книги: {response.StatusCode}");
                return ResponseData<Book>.Error($"Ошибка: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при создании книги: {ex.Message}");
                return ResponseData<Book>.Error($"Ошибка: {ex.Message}");
            }
        }

        // Обновление книги с заменой изображения
        public async Task UpdateProductAsync(int id, Book product, IFormFile? formFile = null)
        {
            try
            {
                var existingResp = await GetProductByIdAsync(id);
                if (!existingResp.Successfull || existingResp.Data == null)
                {
                    _logger.LogError("Не удалось получить книгу для обновления");
                    return;
                }

                // Если новая картинка передана — удаляем старую (кроме noimage) и сохраняем новую
                if (formFile != null)
                {
                    var oldImage = existingResp.Data.Image;
                    if (!string.IsNullOrEmpty(oldImage) && !oldImage.EndsWith("noimage.jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        // Удаляем по имени файла
                        var oldFileName = System.IO.Path.GetFileName(oldImage);
                        await _fileService.DeleteFileAsync(oldFileName);
                    }

                    var imageUrl = await _fileService.SaveFileAsync(formFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var relative = new Uri(imageUrl).LocalPath.TrimStart('/');
                        product.Image = relative;
                        product.ImageMimeType = formFile.ContentType;
                    }
                }
                else
                {
                    // Если файл не меняли — сохраняем прежние значения
                    product.Image = existingResp.Data.Image;
                    product.ImageMimeType = existingResp.Data.ImageMimeType;
                }

                // Убедимся, что отправляем только ключ категории
                if (product.Category != null && product.Category.Id > 0)
                {
                    product.CategoryId = product.Category.Id;
                }
                product.Category = null;

                var response = await _httpClient.PutAsJsonAsync($"books/{id}", product, _serializerOptions);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка при обновлении книги: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обновлении книги: {ex.Message}");
            }
        }

        // Удаление книги
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"books/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Ошибка при удалении книги: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении книги: {ex.Message}");
            }
        }
    }
}
