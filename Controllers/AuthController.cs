using BankingControlPanel.Core;
using BankingControlPanel.DTOs;
using BankingControlPanel.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankingControlPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<ClientController> _logger;
        public AuthController(IAuthService authService, ILogger<ClientController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO model)
        {

            #region Logging 
            _logger.LogInformation("Register Method Invoke : {Request}", Newtonsoft.Json.JsonConvert.SerializeObject(model));
            #endregion

            #region validation
            if (string.IsNullOrWhiteSpace(model.Role))
            {
                return ValidationProblem("Please Enter Role.");
            }

            if (model.Role.ToLowerInvariant() != "user" && model.Role.ToLowerInvariant() != "admin")
            {
                return ValidationProblem("Please Enter Valid Role.");
            }
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return ValidationProblem("Please Enter Email.");
            }

            if (!Validation.IsEmailValid(model.Email))
            {
                return ValidationProblem("Please Enter Valid Email.");
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return ValidationProblem("Please Enter Password.");
            }
            #endregion
            var result = await _authService.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInDTO model)
        {
            #region validation
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return ValidationProblem("Please Enter Email.");
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return ValidationProblem("Please Enter Password.");
            }

           
            #endregion
            var token = await _authService.LoginAsync(model);
            return Ok(new { Token = token });
        }

       
    }
}
