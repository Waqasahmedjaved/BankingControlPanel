using BankingControlPanel.Core;
using BankingControlPanel.DTOs;
using BankingControlPanel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingControlPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly ILogger<ClientController> _logger;
        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddClient(ClientDTO clientDto)
        {
            //Email: Should be required and email format.
            //First name: Should be required and less than 60 characters.
            //Last name: Should be required and less than 60 characters.
            //Mobile number: Should be correct format with country code(you can use some library as well).
            //Personal id: Should be required and it should be exactly 11 characters.
            //Sex: Should be required.
            //Accounts: At least one account is required.

            #region Logging 
            _logger.LogInformation($"Adding a new client: Request , {Newtonsoft.Json.JsonConvert.SerializeObject(clientDto)}");
            #endregion

            #region validation
            if (string.IsNullOrWhiteSpace(clientDto.FirstName))
            {
                return ValidationProblem("Please Enter First Name.");
            }
            if (string.IsNullOrWhiteSpace(clientDto.LastName))
            {
                return ValidationProblem("Please Enter Last Name.");
            }

            if (clientDto.FirstName.Length > 60)
            {
                return ValidationProblem("Please Enter Valid First Name.");
            }
            if (clientDto.LastName.Length > 60)
            {
                return ValidationProblem("Please Enter Valid Last Name.");
            }
            if (string.IsNullOrWhiteSpace(clientDto.Sex))
            {
                return ValidationProblem("Please Enter Sex.");
            }

            if ((string.IsNullOrWhiteSpace(clientDto.Email) || !Validation.IsEmailValid(clientDto.Email)))
            {
                return ValidationProblem("Please Enter Valid Email.");
            }

            if ((string.IsNullOrWhiteSpace(clientDto.MobileNumber) || !Validation.IsValisMobileNumber(clientDto.MobileNumber)))
            {
                return ValidationProblem("Please Enter Valid Mobile Number.");
            }

            if (string.IsNullOrWhiteSpace(clientDto.PersonalId) || clientDto.PersonalId.Length != 11)
            {
                return ValidationProblem("Please Enter Valid Personal Id.");
            }

            if (clientDto.Accounts== null || !clientDto.Accounts.Any())
            {
                return ValidationProblem("Please Enter Account.");
            }
            #endregion

            var result = await _clientService.AddClientAsync(clientDto);
            return Ok(result);
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClients([FromQuery] ClientQueryDTO queryDto)
        {
            #region Logging 
            _logger.LogInformation($"Get Clients call : Request , { Newtonsoft.Json.JsonConvert.SerializeObject(queryDto)}");
            #endregion
            var clients = await _clientService.GetClientsAsync(queryDto);
            return Ok(clients);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClientById(int id)
        {
            if (id < 1)
            {
                return ValidationProblem("Please Enter Valid Id.");
            }
            #region Logging 
            _logger.LogInformation($"Get Clients by id call : Id,{ id}");
            #endregion

            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClient(int id, UpdateClientDTO clientDto)
        {
            #region Logging 
            _logger.LogInformation($"Adding a new client: id = {id} , Request = {Newtonsoft.Json.JsonConvert.SerializeObject(clientDto)}");
            #endregion
            #region validation
            if (id < 1)
            {
                return ValidationProblem("Please Enter Valid Id.");
            }
            if (string.IsNullOrWhiteSpace(clientDto.FirstName))
            {
                return ValidationProblem("Please Enter First Name.");
            }
            if (string.IsNullOrWhiteSpace(clientDto.LastName))
            {
                return ValidationProblem("Please Enter Last Name.");
            }

            if (clientDto.FirstName.Length > 60)
            {
                return ValidationProblem("Please Enter Valid First Name.");
            }
            if (clientDto.LastName.Length > 60)
            {
                return ValidationProblem("Please Enter Valid Last Name.");
            }
            if (string.IsNullOrWhiteSpace(clientDto.Sex))
            {
                return ValidationProblem("Please Enter Sex.");
            }

            if ((string.IsNullOrWhiteSpace(clientDto.Email) || !Validation.IsEmailValid(clientDto.Email)))
            {
                return ValidationProblem("Please Enter Valid Email.");
            }

            if ((string.IsNullOrWhiteSpace(clientDto.MobileNumber) || !Validation.IsValisMobileNumber(clientDto.MobileNumber)))
            {
                return ValidationProblem("Please Enter Valid Mobile Number.");
            }

            if (string.IsNullOrWhiteSpace(clientDto.PersonalId) || clientDto.PersonalId.Length != 11)
            {
                return ValidationProblem("Please Enter Valid Personal Id.");
            }

            if (clientDto.Accounts == null || !clientDto.Accounts.Any())
            {
                return ValidationProblem("Please Enter Account.");
            }
            #endregion
            var updatedClient = await _clientService.UpdateClientAsync(id, clientDto);
            if (updatedClient == null) return NotFound();
            return Ok(updatedClient);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            #region validation
            if (id < 1)
            {
                return ValidationProblem("Please Enter Valid Id.");
            }
            #endregion

            #region Logging 
            _logger.LogInformation($"Delete Client by id call : Id,{ id}");
            #endregion
            var result = await _clientService.DeleteClientAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
