using Microsoft.AspNetCore.Mvc;

namespace Bookify.web.Core.ViewModels.Category
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Max length cannot be more than 5 chr.")]
        [Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = "Category with the same name is already exists !s")]
        public string Name { get; set; } = null!;

    }
}
