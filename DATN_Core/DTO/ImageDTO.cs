﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.DTO
{
 
    public class ImageDTO
    {

        public string Link { get; set; }
    }
    public class CreateImageDTO 
    {
        public IFormFile Picture { get; set; }
    }
    public class UpdateImageDTO 
    {
        public int Id { get; set; }
        public string oldPicture { get; set; }
        public IFormFile Picture { get; set; }

    }
}
