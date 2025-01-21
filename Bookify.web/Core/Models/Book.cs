using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Core.Models
{
    [Index(nameof(Title) , nameof(AuthorId) , IsUnique = true)]
    public class Book
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImagePublicId { get; set; }
        [MaxLength(50)]
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }
        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>(); 

    }
}
