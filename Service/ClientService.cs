using BankingControlPanel.DTOs;
using BankingControlPanel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Service
{
    public class ClientService: IClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientService> _logger;
        public ClientService(ApplicationDbContext context,
            ILogger<ClientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ClientDTO> AddClientAsync(ClientDTO clientDto)
        {
            try
            {
                var client = new Client
                {
                    Email = clientDto.Email,
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    PersonalId = clientDto.PersonalId,
                    ProfilePhoto = clientDto.ProfilePhoto,
                    MobileNumber = clientDto.MobileNumber,
                    Sex = clientDto.Sex,
                    Address = new Address
                    {
                        Country = clientDto.Address.Country,
                        City = clientDto.Address.City,
                        Street = clientDto.Address.Street,
                        ZipCode = clientDto.Address.ZipCode
                    },
                    Accounts = clientDto.Accounts.Select(a => new Account
                    {
                        AccountNumber = a.AccountNumber,
                        Balance = a.Balance
                    }).ToList()
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return clientDto;
            }catch(Exception ex)
            {
                _logger.LogError($"Method AddClientAsync Ex , {ex}");
            }
            return null;
        }

        public async Task<List<ClientDTO>> GetClientsAsync(ClientQueryDTO queryDto)
        {
            try
            {
                IQueryable<Client> query = _context.Clients.Include(c => c.Address).Include(c => c.Accounts);

            // Filtering
            if (!string.IsNullOrEmpty(queryDto.Filter))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(queryDto.Filter) ||
                    c.LastName.Contains(queryDto.Filter) ||
                    c.Email.Contains(queryDto.Filter));
            }

            // Sorting
            if (!string.IsNullOrEmpty(queryDto.SortBy))
            {
                switch (queryDto.SortBy.ToLower())
                {
                    case "email":
                        query = queryDto.SortAscending ? query.OrderBy(c => c.Email) : query.OrderByDescending(c => c.Email);
                        break;
                    case "firstname":
                        query = queryDto.SortAscending ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName);
                        break;
                    case "lastname":
                        query = queryDto.SortAscending ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName);
                        break;
                        // Add other cases as needed
                }
            }

            // Paging
            int skip = (queryDto.PageNumber - 1) * queryDto.PageSize;
            query = query.Skip(skip).Take(queryDto.PageSize);

            var clients = await query.ToListAsync();

            return clients.Select(c => new ClientDTO
            {
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PersonalId = c.PersonalId,
                ProfilePhoto = c.ProfilePhoto,
                MobileNumber = c.MobileNumber,
                Sex = c.Sex,
                Address = new AddressDTO
                {
                    Country = c.Address.Country,
                    City = c.Address.City,
                    Street = c.Address.Street,
                    ZipCode = c.Address.ZipCode
                },
                Accounts = c.Accounts.Select(a => new AccountDTO
                {
                    AccountNumber = a.AccountNumber,
                    RoutingNumber = a.RoutingNumber,
                    Balance = a.Balance
                }).ToList()
            }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method GetClientsAsync Ex , {ex}");
            }
            return null;
        }

        public async Task<ClientDTO> GetClientByIdAsync(int id)
        {
            try { 
            var client = await _context.Clients.Include(c => c.Address).Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return null;

            return new ClientDTO
            {
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PersonalId = client.PersonalId,
                ProfilePhoto = client.ProfilePhoto,
                MobileNumber = client.MobileNumber,
                Sex = client.Sex,
                Address = new AddressDTO
                {
                    Country = client.Address.Country,
                    City = client.Address.City,
                    Street = client.Address.Street,
                    ZipCode = client.Address.ZipCode
                },
                Accounts = client.Accounts.Select(a => new AccountDTO
                {
                    AccountNumber = a.AccountNumber,
                    RoutingNumber = a.RoutingNumber,
                    Balance = a.Balance
                }).ToList()
            };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method GetClientByIdAsync Ex , {ex}");
            }
            return null;
        }

        public async Task<ClientDTO> UpdateClientAsync(int id, UpdateClientDTO clientDto)
        {
            try { 
            var client = await _context.Clients.Include(c => c.Address).Include(c => c.Accounts).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return null;

            client.Email = clientDto.Email;
            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.PersonalId = clientDto.PersonalId;
            client.ProfilePhoto = clientDto.ProfilePhoto;
            client.MobileNumber = clientDto.MobileNumber;
            client.Sex = clientDto.Sex;
            client.Address.Country = clientDto.Address.Country;
            client.Address.City = clientDto.Address.City;
            client.Address.Street = clientDto.Address.Street;
            client.Address.ZipCode = clientDto.Address.ZipCode;
            client.Accounts = clientDto.Accounts.Select(a => new Account
            {
                AccountNumber = a.AccountNumber,
                Balance = a.Balance
            }).ToList();

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return new ClientDTO
            {
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PersonalId = client.PersonalId,
                ProfilePhoto = client.ProfilePhoto,
                MobileNumber = client.MobileNumber,
                Sex = client.Sex,
                Address = new AddressDTO
                {
                    Country = client.Address.Country,
                    City = client.Address.City,
                    Street = client.Address.Street,
                    ZipCode = client.Address.ZipCode
                },
                Accounts = client.Accounts.Select(a => new AccountDTO
                {
                    AccountNumber = a.AccountNumber,
                    RoutingNumber = a.RoutingNumber,
                    Balance = a.Balance
                }).ToList()
            };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method UpdateClientsAsync Ex , {ex}");
            }
            return null;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            try { 
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method DeleteClientsAsync Ex , {ex}");
            }
            return false;
        }
    }
}
