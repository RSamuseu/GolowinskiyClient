﻿namespace GolovinskyAPI.Data.Models.Products
{
    public class NewProductOutputModel
    {
        public char Result { get; set; } = '0';
        public string Prc_ID { get; set; }
        public string Suplier { get; set; }
        public string Cost { get; set; }
        public string gdate { get; set; }
        public string MediaLink { get; set; }
    }
}