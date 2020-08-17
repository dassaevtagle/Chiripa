using System;
using System.ComponentModel.DataAnnotations;
using ChiripaAPI.Models;

namespace ChiripaAPI.Dtos
{
    public class HielitoUpdateDto
    {
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