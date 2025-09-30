using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace BSUIR_LIAGIN.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiCategoryService> _logger;

        public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        /// <summary>
        /// Получение списка категорий из API
        /// </summary>
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                // GET https://localhost:7002/api/categories
                var response = await _httpClient.GetAsync("categories");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content
                        .ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);

                    return data ?? ResponseData<List<Category>>.Error("Пустой ответ от API");
                }

                _logger.LogError($"Ошибка при получении категорий: {response.StatusCode}");
                return ResponseData<List<Category>>.Error($"Ошибка: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Исключение при получении категорий: {ex.Message}");
                return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
            }
        }
    }
}
