using AniGoldShop.Domain.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Common.Class.Configuration.EmailService
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string SenderName { get ; set ; }
        public string SenderEmail { get; set; }

    }
}
