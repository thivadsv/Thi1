using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VTTAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(10, 100000, ErrorMessage = "Giá phải nằm trong khoảng 10 đến 100000")]
        public decimal Price { get; set; }
    }
}
