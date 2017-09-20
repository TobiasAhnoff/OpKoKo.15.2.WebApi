﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int MerchantId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public Language Language { get; set; }
    }
}