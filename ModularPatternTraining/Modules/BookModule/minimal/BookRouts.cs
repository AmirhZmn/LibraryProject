using ModularPatternTraining.Modules.BookModule.DTO;
using ModularPatternTraining.Modules.BookModule.Services;
using ModularPatternTraining.Shared.Services.CacheService;

namespace ModularPatternTraining.Modules.BookModule.minimal
{
    public static class BookRouts
    {
        public static void MapBooksEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/v1/miniBook", async (IBookService service) =>
            {
                var books = await service.GetAllAsync();
                return Results.Ok(books.Data);

            });
            routes.MapGet("/api/v1/miniBook/{id:int}", async (int id, IBookService service, ICacheService cache) =>
            {
                var cacheKey = $"Book_Id:{id}";
                var cachedData = cache.Get<BookDTO>(cacheKey);
                if (cachedData != null)
                {
                    return Results.Ok(cachedData);
                }

                var book = await service.GetById(id);

                if (!book.IsSuccess) return Results.NotFound(book.ErrorMessage);

                cache.Set(cacheKey, book.Data, TimeSpan.FromMinutes(10));
                return Results.Ok(book.Data);


            });
        }
    }
}