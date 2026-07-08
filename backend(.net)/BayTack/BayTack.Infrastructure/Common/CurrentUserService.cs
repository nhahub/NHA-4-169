using BayTack.Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BayTack.Infrastructure.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId
        {
            get
            {
                var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value;
            }
        }

        public string? Email => User?.FindFirstValue(ClaimTypes.Email);

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
    }
}