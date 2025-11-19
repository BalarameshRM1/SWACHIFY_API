using Microsoft.Extensions.Configuration;
using Swachify.Application.Interfaces;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace Swachify.Application;

public class OtpService : IOtpService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _verifyServiceSid;

    public OtpService(IConfiguration configuration)
    {
        _accountSid = configuration["Twilio:AccountSid"];
        _authToken = configuration["Twilio:AuthToken"];
        _verifyServiceSid = configuration["Twilio:VerifyServiceSid"];
        TwilioClient.Init(_accountSid, _authToken);
    }

    public async Task<bool> SendMobileOtpAsync(string phoneNumber)
    {
        var verification = await VerificationResource.CreateAsync(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: _verifyServiceSid
        );
        return verification.Status == "pending";
    }

    public async Task<bool> VerifyMobileOtpAsync(string phoneNumber, string code)
    {
        var verificationCheck = await VerificationCheckResource.CreateAsync(
            to: phoneNumber,
            code: code,
            pathServiceSid: _verifyServiceSid
        );
        return verificationCheck.Status == "approved";
    }

    public async Task<bool> SendCustomerOtpAsync(CustomerOTPDto requestOTP)
    {
        return false;
    }

    public async Task<bool> VerifyCustomerOtpAsync(CustomerOTPDto requestOTP)
    {

        return false;
    }

    private string Generate6DigitOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    
}
