using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ModularPatternTraining.Modules.BookModule.Controllers;
using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Modules.BookModule.Services;
using ModularPatternTraining.Shared.Models;
using ModularPatternTraining.Shared.Services.CacheService;

namespace ModularPatternTraining.Tests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _mockCacheService = new Mock<ICacheService>();
            _controller = new BookController(_mockBookService.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<BookDTO>
            {
                new BookDTO { Id = 1, Name = "Test Book 1" },
                new BookDTO { Id = 2, Name = "Test Book 2" }
            };
            _mockBookService.Setup(service => service.GetAllAsync()).ReturnsAsync(Result<IEnumerable<BookDTO>>.Success(books));

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BookDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetByIdBook_ReturnsOkResult_WithBook()
        {
            // Arrange
            var bookId = 1;
            var book = new BookDTO { Id = bookId, Name = "Test Book" };
            _mockCacheService.Setup(cache => cache.Get<BookDTO>($"Book_Id:{bookId}")).Returns((BookDTO)null);
            _mockBookService.Setup(service => service.GetById(bookId)).ReturnsAsync(Result<BookDTO>.Success(book));

            // Act
            var result = await _controller.GetByIdBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(bookId, returnValue.Id);
        }

        [Fact]
        public async Task GetByIdBook_ReturnsNotFoundResult_WhenBookNotFound()
        {
            // Arrange
            var bookId = 1;
            _mockCacheService.Setup(cache => cache.Get<BookDTO>($"Book_Id:{bookId}")).Returns((BookDTO)null);
            _mockBookService.Setup(service => service.GetById(bookId)).ReturnsAsync(Result<BookDTO>.Failure("Book Not Found",404));

            // Act
            var result = await _controller.GetByIdBook(bookId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteByIdBook_ReturnsOkResult_WhenBookDeleted()
        {
            // Arrange
            var bookId = 1;
            _mockBookService.Setup(service => service.DeleteAsync(bookId)).ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _controller.DeleteByIdBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnValue);
        }

        [Fact]
        public async Task CreateBooks_ReturnsOkResult_WithCreatedBook()
        {
            // Arrange
            var newBook = new BookDTO { Id = 1, Name = "Test Book" };
            _mockBookService.Setup(service => service.AddAsync(newBook)).ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _controller.CreateBooks(newBook);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(newBook.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateBooks_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var updatedBook = new BookDTO { Id = 1, Name = "Updated Book" };
            _mockBookService.Setup(service => service.UpdateAsync(updatedBook)).ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _controller.UpdateBooks(updatedBook);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(updatedBook.Id, returnValue.Id);
        }
    }
}
