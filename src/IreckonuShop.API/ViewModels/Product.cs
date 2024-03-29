﻿using System.ComponentModel.DataAnnotations;

namespace IreckonuShop.API.ViewModels
{
    public class Product
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string ArtikelCode { get; set; }

        [Required]
        public string ColorCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, float.MinValue)]
        public float Price { get; set; }

        [Required]
        [Range(0, 100)]
        public int DiscountPrice { get; set; }

        [Required]
        public string DeliveredIn { get; set; }

        [Required]
        public string Q1 { get; set; }

        [Required]
        [Range(1, 1000)]
        public uint Size { get; set; }

        [Required]
        public string Color { get; set; }
    }
}