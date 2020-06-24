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
        public long Id { get; set; }
        
        /// <summary>
        /// Status of transaction
        /// </summary>
        [Column(TypeName = "nvarchar(10)")]
        public string Status { get; set; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; }

        /// <summary>
        /// Client name of transaction
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        public string ClientName { get; set; }

        /// <summary>
        /// Amount of money
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
