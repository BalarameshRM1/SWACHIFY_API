using System.ComponentModel.DataAnnotations;

namespace Swachify.Application;

public record UserCommandDto
(
    string first_name,
    string last_name,
    string email,
    string mobile,
    string password,
    long role_id,
    long location_id,
    List<long> dept_id
);

public record EmpCommandDto
(
    string first_name,
    string last_name,
    string email,
    string mobile,
    long location_id,
    List<long> dept_id,
    long role_id
);

public record CustomerOTPDto(long otp, long user_id);

public record AssignEmpDto(long id, long user_id);