using System.Threading.Tasks;
using Swachify.Application.Models;

namespace Swachify.Application.Interfaces;

public interface ISMSService
{
    Task<string> SendSMSAsync(SMSRequestDto request);
}
