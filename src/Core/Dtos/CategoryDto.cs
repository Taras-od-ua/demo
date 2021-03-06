using System.Collections.Generic;
using Core.Models;

namespace Core.Dtos
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Color { get; set; }
        public string Title { get; set; }
        public IEnumerable<ListDto> Lists { get; set; }
    }
}
