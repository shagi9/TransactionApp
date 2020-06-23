using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction_API.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName = "nvarchar(10)")]
        public string Status { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string ClientName { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
