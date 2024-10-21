using BlogSharp.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogSharp.Data.Extensions
{
    public class AspnetUser : IAspnetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspnetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUsuarioId() : "";

        }

        public string GetUserRole()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserRole() : "";
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsuarioId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
