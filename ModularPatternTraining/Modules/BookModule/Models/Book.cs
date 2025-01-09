using ModularPatternTraining.Modules.LibraryModule.Models;

namespace ModularPatternTraining.Modules.BookModule.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int? LibraryId { get; set; }
        public Library Library { get; set; }


    }
}
