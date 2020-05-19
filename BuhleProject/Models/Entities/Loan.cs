using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuhleProject.Models.Entities
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string creditor { get; set; }

        public decimal amount { get; set; }

        public DateTime payDate { get; set; }

        public bool paid { get; set; }

    }
}
