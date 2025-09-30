using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using BSUIR_LIAGIN.UI.Services.ProductService;


namespace BSUIR_LIAGIN.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
            builder.Services.AddSingleton(uriData);

            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));

            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
                opt.BaseAddress = new Uri(uriData.ApiUri));

            // регистрация файлового сервиса
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
                opt.BaseAddress = new Uri($"{uriData.ApiUri}files/"));
        }
    }
}
