using Microsoft.AspNetCore.Identity;

namespace HealthcareBillingSystem.Infrastructure.Authentication;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
