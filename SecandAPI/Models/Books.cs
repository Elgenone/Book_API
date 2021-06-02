using System;
using System.Collections.Generic;

namespace SecandAPI.Models
{
    public partial class Books
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Catagory { get; set; }
        public string Auther { get; set; }
        public decimal BookPrice { get; set; }
        public int Quantity { get; set; }
    }
}
