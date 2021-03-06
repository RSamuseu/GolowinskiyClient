﻿using System;

namespace GolovinskyAPI.Data.Models.Background
{
    public class Background
    {
        public string AppCode { get; set; }

        public DateTime Date { get; set; }

        public string FileName { get; set; }

        public byte[] Img { get; set; }

        public char Mark { get; set; }

        public char orient { get; set; }

        public char place { get; set; }
    }
}
