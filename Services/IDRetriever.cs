using System.Security.Claims;

namespace WorkProj.Services
{
    public class IDRetriever
    {

        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(typeof(IDRetriever));

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IDRetriever(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetLoggedInUserId()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogInformation("Користувач є залогіненим");
                return userId;
            }

            _logger.LogInformation("Користувач не є залогіненим");
            return 0;
        }
    }
}
