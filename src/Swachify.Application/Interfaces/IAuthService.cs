using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IAuthService
{
    Task<user_registration?> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default);
    Task<string> ForgotPasswordAsync(string email, string newPassword, string confirmPassword, CancellationToken ct = default);
    Task<bool> ForgotpasswordLink(long user_id);
    
}
