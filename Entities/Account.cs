using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }
    }
}
