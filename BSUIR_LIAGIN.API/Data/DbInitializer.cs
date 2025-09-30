using Microsoft.EntityFrameworkCore;
using BSUIR_LIAGIN.Domain.Entities;

namespace BSUIR_LIAGIN.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // применяем миграции
            await context.Database.MigrateAsync();

            // если категорий нет — добавляем
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Антиутопия", NormalizedName = "dystopia" },
                    new Category { Name = "Классическая драма", NormalizedName = "classical-drama" },
                    new Category { Name = "Исторический роман", NormalizedName = "historical-novels" },
                    new Category { Name = "Фэнтези", NormalizedName = "fantasy" },
                    new Category { Name = "Приключения", NormalizedName = "adventure" },
                    new Category { Name = "Детектив", NormalizedName = "detective" },
                    new Category { Name = "Магический реализм", NormalizedName = "magical-realism" },
                    new Category { Name = "Русская классика", NormalizedName = "russian-classics" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // если книг нет — добавляем
            if (!context.Books.Any())
            {
                var categories = context.Categories.ToList();

                var books = new List<Book>
                {
                    new Book
                    {
                        Name = "1984",
                        Description = "Антиутопия Джорджа Оруэлла",
                        Category = categories.First(c => c.NormalizedName == "dystopia"),
                        Price = 15.99m,
                        Image = "Images/1984.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Горе от ума",
                        Description = "Классическая пьеса Александра Грибоедова",
                        Category = categories.First(c => c.NormalizedName == "classical-drama"),
                        Price = 9.99m,
                        Image = "Images/woe_from_mind.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Война и мир",
                        Description = "Исторический роман Льва Толстого",
                        Category = categories.First(c => c.NormalizedName == "historical-novels"),
                        Price = 25.50m,
                        Image = "Images/war_and_peace.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Гарри Поттер",
                        Description = "Фэнтези роман Дж. К. Роулинг",
                        Category = categories.First(c => c.NormalizedName == "fantasy"),
                        Price = 18.00m,
                        Image = "Images/harry_potter.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Три мушкетера",
                        Description = "Приключенческий роман Александра Дюма",
                        Category = categories.First(c => c.NormalizedName == "adventure"),
                        Price = 14.00m,
                        Image = "Images/three_musketeers.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Шерлок Холмс",
                        Description = "Детективные истории Артура Конан Дойла",
                        Category = categories.First(c => c.NormalizedName == "detective"),
                        Price = 12.00m,
                        Image = "Images/sherlock.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Мастер и Маргарита",
                        Description = "Магический реализм Михаила Булгакова",
                        Category = categories.First(c => c.NormalizedName == "magical-realism"),
                        Price = 20.00m,
                        Image = "Images/master_and_margarita.jpg",
                        ImageMimeType = "image/jpeg"
                    },
                    new Book
                    {
                        Name = "Преступление и наказание",
                        Description = "Роман Фёдора Достоевского",
                        Category = categories.First(c => c.NormalizedName == "russian-classics"),
                        Price = 16.00m,
                        Image = "Images/crime_and_punishment.jpg",
                        ImageMimeType = "image/jpeg"
                    }
                };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }
        }
    }
}
