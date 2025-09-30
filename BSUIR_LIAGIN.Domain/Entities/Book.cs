using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSUIR_LIAGIN.Domain.Entities
{
        public class Book
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;        // Название книги
            public string? Description { get; set; }                 // Описание           
            public int CategoryId { get; set; }
            public Category? Category { get; set; }                  // Навигация 
            public decimal Price { get; set; }                       // Цена 
            public string? Image { get; set; }                       // Путь
            public string? ImageMimeType { get; set; }               // Mime тип 
       
    }
    
}
