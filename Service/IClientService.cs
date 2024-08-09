using BankingControlPanel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Service
{
    public interface IClientService
    {
        Task<ClientDTO> AddClientAsync(ClientDTO clientDTO);
        Task<List<ClientDTO>> GetClientsAsync(ClientQueryDTO queryDTO);
        
        Task<ClientDTO> GetClientByIdAsync(int id);
        Task<ClientDTO> UpdateClientAsync(int id, UpdateClientDTO clientDto);
        Task<bool> DeleteClientAsync(int id);
    }
}
