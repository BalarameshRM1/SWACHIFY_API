using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Swachify.Application.DTOs;
using Swachify.Application.Interfaces;
using Swachify.Infrastructure.Data;
using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public class UserService(MyDbContext db, IPasswordHasher hasher, IEmailService email, IConfiguration config) : IUserService
{

    public async Task<long> CreateUserAsync(UserCommandDto cmd, CancellationToken ct = default)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(cmd.email))
            throw new ArgumentException("Email is required", nameof(cmd.email));

        if (await db.user_registrations.AnyAsync(u => u.email == cmd.email, ct))
            throw new InvalidOperationException("Email exists");
        long userid = await db.user_registrations.MaxAsync(u => (long?)u.id) ?? 0L;
        userid = userid + 1;
        var user = new user_registration
        {
            id = userid,
            email = cmd.email,
            first_name = cmd.first_name,
            last_name = cmd.last_name,
            mobile = cmd.mobile,
            role_id = cmd.role_id,
            location_id = cmd.location_id,
        };

        await using var tx = await db.Database.BeginTransactionAsync(ct);

        try
        {
            await db.user_registrations.AddAsync(user, ct);
            await db.SaveChangesAsync(ct);

            if (cmd.dept_id != null && cmd.dept_id.Any())
            {
                var userDeptList = new List<user_department>(cmd.dept_id.Count);
                foreach (var d in cmd.dept_id)
                {
                    userDeptList.Add(new user_department
                    {
                        user_id = userid,
                        dept_id = d
                    });
                }

                if (userDeptList.Count > 0)
                {
                    await db.user_departments.AddRangeAsync(userDeptList, ct);
                }
            }

            var userAuth = new user_auth
            {
                id = userid,
                user_id = userid,
                email = cmd.email,
                password = hasher.Hash(cmd.password)
            };

            await db.user_auths.AddAsync(userAuth, ct);

            await db.SaveChangesAsync();

            await tx.CommitAsync(ct);
            


            return user.id;
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                throw new InvalidOperationException("Email already exists (unique constraint)", dbEx);
            }

            throw;
        }
    }

    public async Task<List<AllUserDtos>> GetAllUsersAsync(AllusersDto cmd)
    {
       var userid = cmd.userid > 0 ? cmd.userid : -1;
        var roleid = cmd.roleid > 0 ? cmd.roleid : -1;
        var query = string.Format(DbConstants.fn_users_list, userid, roleid, cmd.limit, cmd.offset);
        var rawData = await db.Database.SqlQueryRaw<AllUserDtos>(query).ToListAsync();
        return rawData.ToList();
    }
    public async Task<AllUserDtos> GetUserByID(long id)
    {
        var query = string.Format(DbConstants.fn_users_list, id, -1, 1, 0);
        var rawData = await db.Database.SqlQueryRaw<AllUserDtos>(query).ToListAsync();
        return rawData.FirstOrDefault();
    }
    private async Task SendWelcomeEmailAsync(string toEmail, string firstName, string password)
    {
        string subject = "Welcome to Swachify - Your Account Details";

        string body = $@"
    <html>
        <body>
            <h2>Welcome to Swachify, {firstName}!</h2>
            <p>Your account has been created successfully.</p>
            <p><strong>Login Email:</strong> {toEmail}</p>
            <p><strong>Temporary Password:</strong> {password}</p>
            <p>
                For your security, please change your password immediately after logging in.
            </p>
            <p>
                <a href='https://yourdomain.com/change-password' target='_blank'>
                    Click here to change your password
                </a>
            </p>
            <br/>
            <p>Thank you,<br/>The Swachify Team</p>
        </body>
    </html>";

        await email.SendEmailAsync(toEmail, subject, body);
    }


    public async Task<long> CreateEmployeAsync(EmpCommandDto cmd, CancellationToken ct = default)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(cmd.email))
            throw new ArgumentException("Email is required", nameof(cmd.email));

        if (await db.user_registrations.AnyAsync(u => u.email == cmd.email, ct))
            throw new InvalidOperationException("Email exists");
        long userid = await db.user_registrations.MaxAsync(u => (long?)u.id) ?? 0L;
        userid = userid + 1;
        var user = new user_registration
        {
            id = userid,
            email = cmd.email,
            first_name = cmd.first_name,
            last_name = cmd.last_name,
            mobile = cmd.mobile,
            role_id = cmd.role_id,
            location_id = cmd.location_id,
            is_active = true,
        };

        await using var tx = await db.Database.BeginTransactionAsync(ct);

        try
        {
            await db.user_registrations.AddAsync(user, ct);
            await db.SaveChangesAsync(ct);

            if (cmd.dept_id != null && cmd.dept_id.Any())
            {
                var userDeptList = new List<user_department>(cmd.dept_id.Count);
                foreach (var d in cmd.dept_id)
                {
                    userDeptList.Add(new user_department
                    {
                        user_id = userid,
                        dept_id = d
                    });
                }

                if (userDeptList.Count > 0)
                {
                    await db.user_departments.AddRangeAsync(userDeptList, ct);
                }
            }

            var userAuth = new user_auth
            {
                id = userid,
                user_id = userid,
                email = cmd.email,
                is_active = true,
                password = hasher.Hash(cmd.email)
            };

            await db.user_auths.AddAsync(userAuth, ct);

            await db.SaveChangesAsync();

            await tx.CommitAsync(ct);
            await SendWelcomeEmailAsync(cmd.email, cmd.first_name, cmd.email);

            // Email template
            var resetlink = config["ResetPasswordLink"] + user.id;
            var mailtemplate = await db.email_templates.
            FirstOrDefaultAsync(b => b.title == AppConstants.ResetEmail);
            string emailBody = mailtemplate.description
                .Replace("{0}", user?.first_name + " " + user?.last_name)
                .Replace("{resetLink}", resetlink);
            if (mailtemplate != null)
            {
                await email.SendEmailAsync(user?.email, AppConstants.ResetEmail, emailBody);
            }
            return user.id;
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                throw new InvalidOperationException("Email already exists (unique constraint)", dbEx);
            }
            throw;
        }
    }


    

    public async Task<bool> UpdateUserAsync(long id, EmpCommandDto cmd)
    {
        var existing = await db.user_registrations.FirstOrDefaultAsync(b => b.id == id);
        if (existing == null) return false;

        existing.email = cmd.email;
        existing.first_name = cmd.first_name;
        existing.last_name = cmd.last_name;
        existing.mobile = cmd.mobile;
        existing.role_id = cmd.role_id;
        existing.location_id = cmd.location_id;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(long id)
    {
        var existing = await db.user_registrations.FirstOrDefaultAsync(b => b.id == id);
        if (existing == null) return false;
        existing.is_active = false;
        await db.SaveChangesAsync();
        return true;
    }
}