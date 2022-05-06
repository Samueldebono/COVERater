﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVERater.API.Dto
{
    public class ImageDto
    {
        public int ImageId { get; set; }
        public Guid CloudinaryId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public decimal DecayRate { get; set; }
    }
}
