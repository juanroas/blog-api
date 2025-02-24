using Blog.Application.Services.Interfaces;
using Blog.Domain.Models;
using Blog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> RegisterUser(string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return (false, "Usuário já existe.");

            var user = new User { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded ? (true, "Usuário registrado com sucesso.") : (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            return result.Succeeded;
        }
    }
}
