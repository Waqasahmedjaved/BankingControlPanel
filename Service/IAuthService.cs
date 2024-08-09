using BankingControlPanel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Service
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegistrationDTO model);
        Task<string> LoginAsync(LogInDTO model);
    }
}
