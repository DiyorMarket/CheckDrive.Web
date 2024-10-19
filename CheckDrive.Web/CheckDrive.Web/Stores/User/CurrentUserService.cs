using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CheckDrive.Web.Stores.User;

public class CurrentUserService:ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetRole()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null || !httpContext.Request.Cookies.ContainsKey("tasty-cookies"))
        {
            return null; 
        }

        string token = httpContext.Request.Cookies["tasty-cookies"];
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        var roleId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

        return roleId;
    }
}
