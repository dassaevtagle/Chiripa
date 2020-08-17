using System;
using System.ComponentModel.DataAnnotations;

namespace ChiripaAPI.Models
{
    public class Hielito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public Category Category { get; set; }
        
        [Required]
        public Boolean InStock { get; set; }
    }
}