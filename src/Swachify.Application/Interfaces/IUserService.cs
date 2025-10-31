using Swachify.Application.DTOs;
using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IUserService
{
    Task<long> CreateUserAsync(UserCommandDto cmd, CancellationToken ct = default);

    Task<List<AllUserDtos>> GetAllUsersAsync();

    Task<List<user_registration>> GetAllUsersByDept(long deptId);
    Task<AllUserDtos> GetUserByID(long id);

    Task<long> CreateEmployeAsync(EmpCommandDto cmd, CancellationToken ct = default);



    Task<bool> DeleteUserAsync(long id);
}