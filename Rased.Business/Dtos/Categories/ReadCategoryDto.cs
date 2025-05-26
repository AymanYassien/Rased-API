using System;
using System.Collections.Generic;
using Rased.Business.Dtos.SubCategories;

namespace Rased.Business.Dtos.Categories
{
    public class ReadCategoryDto: CategoryDto
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Subs
        public List<ReadSubCategoryDto>? SubCategories { get; set; }
    }
}
