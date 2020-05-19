using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuhleProject.Models.Entities
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int UploaderId { get; set; }
        public string filename { get; set; }

        public DateTime DateUploaded { get; set; }

        public string DocType { get; set; }

        public string extension { get; set; }

        public byte[] document { get; set; }

        public string path { get; set; }
    }
}
