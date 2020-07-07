using System;
using System.Collections.Generic;
using System.Text;

namespace TA.Business.Models
{
    public class AddOrUpdateTransactionVm
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
    }
}
