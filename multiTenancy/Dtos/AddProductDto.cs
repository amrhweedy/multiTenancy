﻿namespace multiTenancy.Dtos
{
    public class AddProductDto
    {
        public string Name { get; set; } = null!; 
        public string Description { get; set; } = null!;
        public int Rate { get; set; }
    }
}
