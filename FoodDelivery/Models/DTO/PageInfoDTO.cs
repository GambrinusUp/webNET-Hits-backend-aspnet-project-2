using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTO
{
    public class PageInfoDTO
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int PageSize { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int PageCount { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int CurrentPage { get; set; }
    }
}
