using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Interfaces.Configuration
{
    public interface IUploadFileConfiguration
    {
        public long MaximumSmallFileSize { get; set; }
    }
}
