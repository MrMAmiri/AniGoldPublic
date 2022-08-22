using Ghasedak.Core.Exceptions;
using AniGoldShop.Domain.Common.Exceptions.SMSService;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using System.Threading.Tasks; 

namespace AniGoldShop.Infrastructure.SMSService.GhasedakSMSService
{
    public class GhasedakSMS : ISmsService
    {
        private string _apiKey;
        private string _verifySMSTemplate;

        public GhasedakSMS(string apiKey, string verifySMSTemplate)
        {
            _apiKey = apiKey;
            _verifySMSTemplate = verifySMSTemplate;
        }

        public GhasedakSMS(string apiKey)
        {
            _apiKey = apiKey;
        }

        public GhasedakSMS()
        {
            _apiKey = string.Empty;
        }

        public async Task SendSMS(string message, string phoneNumber)
        {
            if (_apiKey == null || _apiKey == string.Empty)
                throw new SMSServiceAPIKeyEmptyException("SMS API Key is empty");

            try
            {
                var sms = new Ghasedak.Core.Api(_apiKey);
                var result = await sms.SendSMSAsync(message, phoneNumber);
            }
            catch (ApiException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
            catch (ConnectionException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
        }

        public async Task SendVerifySMS(string phoneNumber, string verifyCode)
        {
            if (_apiKey == null || _apiKey == string.Empty)
                throw new SMSServiceAPIKeyEmptyException("SMS API Key is empty");

            try
            {
                var smsService = new Ghasedak.Core.Api(_apiKey);
                var result = await smsService.VerifyAsync(1, _verifySMSTemplate, new string[] { phoneNumber }, param1: verifyCode);
            }
            catch (ApiException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
            catch (ConnectionException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
        }

        public void SetAPIKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void SetVerifySMSTemplate(string verifySMSTemplate)
        {
            _verifySMSTemplate = verifySMSTemplate;
        }

        public void SetAPIKey(string usern, string pass, string pattern)
        {
            throw new System.NotImplementedException();
        }

        public Task SendSMS(string phoneNumber, string docNumber, string cutting, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task SendSMS(string phoneNumber, string docNumber, string cutting, string name, int priot, string workId,bool call)
        {
            throw new System.NotImplementedException();
        }
    }
}
