using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Entities
{
    public class Client
    {
        //[Key]
        //public int Id { get; set; }
        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string PersonalId { get; set; }
        //public string ProfilePhoto { get; set; }
        //public string MobileNumber { get; set; }
        //public string Sex { get; set; }
        //public Address Address { get; set; }
        //public ICollection<Account> Accounts { get; set; }


        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        [Required]
        [StringLength(11)]
        public string PersonalId { get; set; }

        public string ProfilePhoto { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        public string Sex { get; set; }

        public Address Address { get; set; }

        [Required]
        public ICollection<Account> Accounts { get; set; }

    }
}
