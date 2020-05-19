using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuhleProject.Models.Entities
{
    public class LoanApplication
    {
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [DisplayName("application Date")]
        public DateTime applicationDate { get; set; }
        [DisplayName("estimated repayment date")]
        public DateTime estPayDate { get; set; }
        public string applicant { get; set; }
        public decimal amount { get; set; }
        public bool approved { get; set; }
        public bool rejected { get; set; }

        public string reason { get; set; }
    }
}
