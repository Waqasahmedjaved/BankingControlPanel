using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.DTOs
{
    public class AccountDTO
    {
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
