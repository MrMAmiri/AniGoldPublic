using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Interfaces.Configuration
{
    public interface IEmailSmtpConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
    }
}
