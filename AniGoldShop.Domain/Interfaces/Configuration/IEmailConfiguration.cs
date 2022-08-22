using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Interfaces.Configuration
{
    public interface IEmailConfiguration
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }
}
