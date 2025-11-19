using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swachify.Application.Interfaces
{
    public interface IOtpService
    {
        Task<bool> SendMobileOtpAsync(string phoneNumber);
        Task<bool> VerifyMobileOtpAsync(string phoneNumber, string code);

        Task<bool> SendCustomerOtpAsync(CustomerOTPDto requestOTP);

        Task<bool> VerifyCustomerOtpAsync(CustomerOTPDto requestOTP);

        // Task<string> SendCustomerOtpAsync(CustomerOTPDto requestOTP);


    }
}
