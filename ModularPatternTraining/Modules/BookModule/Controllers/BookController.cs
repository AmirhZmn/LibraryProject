using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Modules.BookModule.Services;
using ModularPatternTraining.Shared.Services.CacheService;

namespace ModularPatternTraining.Modules.BookModule.Controllers
{

    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ICacheService _cacheService;

        public BookController(IBookService bookService, ICacheService cacheService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
        }


        [HttpGet("AllBooks")]
        public async Task<IActionResult>  GetAllBooks()
        {

            var books = await _bookService.GetAllAsync();
            if (books.IsSuccess) return Ok(books.Data);
            return books.StatusCode == 500 ? Problem(books.ErrorMessage) : NotFound(books.ErrorMessage);
        }


        [HttpGet("Book")]
        public async Task<IActionResult> GetByIdBook(int id)
        {
            var cacheKey = $"Book_Id:{id}";
            var cachedData = _cacheService.Get<BookDTO>(cacheKey);
            if (cachedData != null)
            {
                return Ok(cachedData);
            }

            var book = await _bookService.GetById(id);
           
            if (!book.IsSuccess) return NotFound(book.ErrorMessage);

            _cacheService.Set(cacheKey, book.Data, TimeSpan.FromMinutes(10));
            return Ok(book.Data);



        }
        
         [HttpGet("DeleteBook")]
        public async Task<IActionResult> DeleteByIdBook(int id)
        {
            var book = await _bookService.DeleteAsync(id);
            if (book.IsSuccess) return Ok(book.Data);

            return book.StatusCode == 500 ? Problem(book.ErrorMessage) : NotFound(book.ErrorMessage);
            
        }

        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBooks([FromBody] BookDTO newBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var result =  await _bookService.AddAsync(newBook);
            if (result.IsSuccess) return Ok(result.Data);
            

            return  BadRequest(result.ErrorMessage);

        }

        [HttpPost("UpdateBook")]
        public async Task<IActionResult> UpdateBooks([FromBody] BookDTO newBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bookService.UpdateAsync(newBook);
            if (result.IsSuccess) return Ok(result.Data);

            return result.StatusCode == 500 ? Problem(result.ErrorMessage) : BadRequest(result.ErrorMessage);
           
        }
    }
}
