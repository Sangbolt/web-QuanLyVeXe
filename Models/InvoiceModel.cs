using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VEXE.Models
{
    public class InvoiceModel
    {
        // Order details
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int GuestTotal { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }

        // Ticket details
        public decimal Price { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
    }
}