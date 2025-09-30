using BSUIR_LIAGIN.UI.Extensions;
using BSUIR_LIAGIN.UI.Services.CategoryService;
using BSUIR_LIAGIN.UI.Services.ProductService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


// читаем UriData из appsettings.json
var uriData = builder.Configuration.GetSection("UriData").Get<BSUIR_LIAGIN.Domain.Entities.UriData>();
builder.Services.AddSingleton(uriData);

// регистрируем HttpClient для сервисов
builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
    opt.BaseAddress = new Uri(uriData.ApiUri));



// твои кастомные сервисы (если есть)
builder.RegisterCustomServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
