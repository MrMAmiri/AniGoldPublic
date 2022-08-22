using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Domain.Interfaces.ExternalServices
{
    public interface ISmsService
    {
        void SetVerifySMSTemplate(string apiKey);
        void SetAPIKey(string apiKey);
        void SetAPIKey(string usern, string pass, string pattern);
        Task SendSMS(string message, string phoneNumber);
        Task SendSMS(string phoneNumber, string docNumber, string cutting, string name);
        Task SendSMS(string phoneNumber, string docNumber, string cutting, string name, int priot, string workId,bool call);
        Task SendVerifySMS(string phoneNumber, string verifyCode);
    }
}
