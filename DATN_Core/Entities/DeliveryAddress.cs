﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Core.Entities
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public int ZipCode {  get; set; }
        public int Phone { get; set; }
        public string Note { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
