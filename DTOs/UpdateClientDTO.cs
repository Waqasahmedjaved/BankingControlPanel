using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.DTOs
{
    public class UpdateClientDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string ProfilePhoto { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public AddressDTO Address { get; set; }
        public List<AccountDTO> Accounts { get; set; }
    }
}
