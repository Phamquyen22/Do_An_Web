using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoanWeb.Models
{
    public class productviewpage
    {
        
            public List<Products> Products { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
        public int Totalproduct { get; set; }

    }
}