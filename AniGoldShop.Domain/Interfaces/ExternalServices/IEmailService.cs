using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Domain.Interfaces.ExternalServices
{
    public interface IEmailService
    {
        Task SendTextMessage(string subject, string messgae);
        Task SendHtmlMessage(string subject, string htmlMessage);
        void SetReceptorEmailAddress(string name, string emailAddress);
        void SetSenderEmailAddress(string name, string emailAddress);
    }
}
