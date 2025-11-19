namespace Swachify.Application.Models;

public record EmailRequestDto(string To);
public record SMSRequestDto(string To,string message);
