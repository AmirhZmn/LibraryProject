using System.ComponentModel.DataAnnotations;

namespace ModularPatternTraining.Modules.BookModule.DTO
{
    public class BookDTO
    {
       
        
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(100)]
        public string Author { get; set; }

    }
}
