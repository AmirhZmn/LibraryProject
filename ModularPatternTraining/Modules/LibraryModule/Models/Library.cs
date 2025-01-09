using ModularPatternTraining.Modules.BookModule.Models;

namespace ModularPatternTraining.Modules.LibraryModule.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string State { get; set; }
        public string City { get; set; }

        public DateTime AddDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }


        public string description { get; set; }

        public bool IsRemoved { get; set; }

        public List<Book> Books { get; set; }

    }
}
