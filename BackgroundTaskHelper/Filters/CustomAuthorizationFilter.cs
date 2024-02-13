using BackgroundTaskHelper.Models;
using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace BackgroundTaskHelper.Filters
{
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
