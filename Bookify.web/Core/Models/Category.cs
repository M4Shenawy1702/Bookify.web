﻿using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedOn { get; set; }
        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();

    }
}
