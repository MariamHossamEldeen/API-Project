using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Quantity Must Be Greater Than Zero")]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0.1 , double.MaxValue , ErrorMessage ="Price Must Be Greater Than Zero")]
        public decimal Price { get; set; }
        
        [Required]
        public string PictureUrl { get; set; }
        
        [Required]
        public string Brand { get; set; }
        
        [Required]
        public string Type { get; set; }

    }
}