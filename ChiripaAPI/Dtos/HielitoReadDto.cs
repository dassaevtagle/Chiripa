using System;
using ChiripaAPI.Models;

namespace ChiripaAPI.Dtos
{
    public class HielitoReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public Boolean InStock { get; set; }
    }
}