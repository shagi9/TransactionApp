using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction_API.Dto
{
    public class EditStatusDto
    {
        /// <summary>
        /// Status of transaction
        /// </summary>
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Status { get; set; }
    }
}
