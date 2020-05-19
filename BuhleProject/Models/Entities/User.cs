using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuhleProject.Models.Entities
{
    public class User
    {
        public User()
        {

        }
        public User(bool place)
        {
            Accepted = this.completed2 = this.completed3 = completed3 = this.pBank = pBank = this.pReg = pReg = this.pID = pID = this.pStdCard = pStdCard = this.pAddress = pAddress = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Institution { get; set; }
        public bool Accepted { get; set; }
        public string address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool completed1 { get; set; }
        public bool completed2 { get; set; }
        public bool completed3{ get; set; }

        //[Docs]
        public bool pBank { get; set; }
        public bool pReg { get; set; }
        public bool pID{ get; set; }
        public bool pStdCard { get; set; }
        public bool pAddress { get; set; }
    }
}
