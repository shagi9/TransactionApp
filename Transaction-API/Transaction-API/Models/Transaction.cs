using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction_API.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
    }
}
