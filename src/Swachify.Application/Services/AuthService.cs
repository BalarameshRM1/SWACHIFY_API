using Microsoft.AspNetCore.Identity;
using Swachify.Infrastructure.Models;
using Swachify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Swachify.Application.Interfaces;
using Microsoft.Extensions.Configuration;
namespace Swachify.Application;

public class AuthService(MyDbContext db, IPasswordHasher hasher, IEmailService emailService, IConfiguration config) : IAuthService
{
    public async Task<user_registration?> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default)
    {
        var user_auth = await db.user_auths.FirstOrDefaultAsync(u => u.email == email);
        if (user_auth is null) return null;
        var user_reg = await db.user_registrations.FirstOrDefaultAsync(u => u.email == email);
        if (user_reg != null)
        {
            user_reg.user_authusers = new List<user_auth>();
        }
        return hasher.Verify(password, user_auth.password) ? user_reg : null;
    }
   
    public async Task<string> ForgotPasswordAsync(long? id, string newPassword, string confirmPassword, CancellationToken ct = default)
    {

        if (id>0 || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            return "Email and passwords are required.";

        if (newPassword != confirmPassword)
            return "Password and Confirm Password do not match.";

        var userAuth = await db.user_auths.FirstOrDefaultAsync(u => u.id == id, ct);
        if (userAuth == null)
            return "Email not found. Please check your email or register first.";


        userAuth.password = hasher.Hash(newPassword);
        userAuth.modified_date = DateTime.Now;


        db.user_auths.Update(userAuth);
        await db.SaveChangesAsync(ct);

        return "Password updated successfully.";
    }

    public async Task<bool> ForgotpasswordLink(string user_email)
    {
        var user = await db.user_registrations.FirstOrDefaultAsync(d => d.email.ToUpper() == user_email.ToUpper());
        if (user is null) return false;
        var resetlink = config["ResetPasswordLink"] + user?.id;
        var mailtemplate = await db.booking_templates.
        FirstOrDefaultAsync(b => b.title == AppConstants.ResetEmail);
        string emailBody = mailtemplate.description
            .Replace("{0}", user?.first_name + " " + user?.last_name)
            .Replace("{resetLink}", resetlink);
        if (mailtemplate != null)
        {
            await emailService.SendEmailAsync(user_email, AppConstants.ResetEmail, emailBody);
        }
        return true;
    }
}
