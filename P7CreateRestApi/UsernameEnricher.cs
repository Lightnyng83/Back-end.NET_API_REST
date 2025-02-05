using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Security.Claims;

namespace P7CreateRestApi
{
   public class UsernameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsernameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Username", username));
        }
        else
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Username", "Anonymous"));
        }
    }
}


}
