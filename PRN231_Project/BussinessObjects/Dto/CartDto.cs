﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Dto
{
    public class CartDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int SubPrice => Price * Quantity;
    }
}