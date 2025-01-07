using Bookify.web.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.web.Core.ViewModels.Book
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(500)]
        [Remote("AllowItem", null, AdditionalFields = "Id,AuthorId", ErrorMessage = "Book with the same name is already exists !s")]
        public string Title { get; set; } = null!;
        [Display(Name ="Author")]
        [Remote("AllowItem", null, AdditionalFields = "Id,Title", ErrorMessage = "Book with the same name is already exists !s")]
        public int AuthorId { get; set; }
        //to show selectlist of Authors
        public IEnumerable<SelectListItem>? Authors { get; set; }
        public int PublisherId { get; set; }
        //to show selectlist of Publishers
        public IEnumerable<SelectListItem>? Publishers { get; set; }

        [Display(Name = "Publishing Date")]
        [AssertThat("PublishingDate <= Today()")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        //for dispaly the image when Edit or somthing else
        public string? ImageUrl { get; set; }
        [MaxLength(50)]
        public string Hall { get; set; } = null!;
        [Display(Name = "Is Available For Rental ?")]
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        //Use Ilist becacuse i need the index in the controller 
        public IList<int> SelectedCategories { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
