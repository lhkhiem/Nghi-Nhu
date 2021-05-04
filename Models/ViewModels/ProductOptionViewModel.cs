using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ProductOptionViewModel
    {
        public long ProductID { get; set; }
        public long OptionID { get; set; }
        public string OptionName { get; set; }
        public decimal Price { get; set; }
    }
}