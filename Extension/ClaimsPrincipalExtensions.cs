using System.Security.Claims;

namespace ToDo_App.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            // Find the NameIdentifier claim (maps to 'sub' in JWT)
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
            {
                return id;
            }

            return 0; // Return 0 or throw an exception if user is not found
        }
    }
}