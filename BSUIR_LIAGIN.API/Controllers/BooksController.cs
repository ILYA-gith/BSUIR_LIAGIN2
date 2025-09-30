using Microsoft.AspNetCore.Mvc;
using BSUIR_LIAGIN.API.Services;
using BSUIR_LIAGIN.Domain.Entities;
using BSUIR_LIAGIN.Domain.Models;

namespace BSUIR_LIAGIN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IProductService _productService;

        public BooksController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Book>>>> GetBooks(
            string? category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
        }


        // GET: api/Books/fantasy?pageNo=2
        [HttpGet("{category}")]
        public async Task<ActionResult<ResponseData<ListModel<Book>>>> GetBooksByCategory(
            string category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
        }

        [HttpGet("{id:int}")]
    
        public async Task<ActionResult<ResponseData<Book>>> GetBook(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Successfull)
                return NotFound(result.ErrorMessage);

            return Ok(result);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<ResponseData<Book>>> PostBook(Book book)
        {
            var result = await _productService.CreateProductAsync(book);
            if (!result.Successfull)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetBook), new { id = result.Data!.Id }, result);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest("Id mismatch");

            var existing = await _productService.GetProductByIdAsync(id);
            if (!existing.Successfull)
                return NotFound(existing.ErrorMessage);

            await _productService.UpdateProductAsync(id, book);
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existing = await _productService.GetProductByIdAsync(id);
            if (!existing.Successfull)
                return NotFound(existing.ErrorMessage);

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }

}
