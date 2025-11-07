using Swachify.Application.DTOs;
using Swachify.Infrastructure.Models;

namespace Swachify.Application;

public interface IUserService
{
    Task<long> CreateUserAsync(UserCommandDto cmd, CancellationToken ct = default);

    Task<List<AllUserDtos>> GetAllUsersAsync(AllusersDto cmd);

    Task<AllUserDtos> GetUserByID(long id);

    Task<long> CreateEmployeAsync(EmpCommandDto cmd, CancellationToken ct = default);
    Task<bool> UpdateUserAsync(long id, EmpCommandDto cmd);
    Task<bool> DeleteUserAsync(long id);
}