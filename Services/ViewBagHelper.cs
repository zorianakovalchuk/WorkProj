using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace WorkProj.Services
{
    public static class ViewBagHelper
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(typeof(ViewBagHelper));

        public static void SetIsLoggedInInViewBag(this ViewContext viewContext)
        {
            bool isLoggedIn = viewContext.HttpContext.User.Identity.IsAuthenticated;
            viewContext.ViewBag.IsLoggedIn = isLoggedIn;
            _logger.LogInformation("Успішно перевірено чи користувач є залогіненим");
        }
    }
}
