using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Northwind.EF_Task2_
{
    public class CreditCard
    {
        public int CreditCardID { get; set; }

        [StringLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [StringLength(200)]
        public string CardHolderName { get; set; }

        public int EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
