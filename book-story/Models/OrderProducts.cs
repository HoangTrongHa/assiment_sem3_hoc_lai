using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookOnlineShop.Models
{
    public class OrderProducts
    {
        public Orders Orders { get; set; }
        public Products Products { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }

        public int quantity { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]

        public string UserID { get; set; }
    }
}
