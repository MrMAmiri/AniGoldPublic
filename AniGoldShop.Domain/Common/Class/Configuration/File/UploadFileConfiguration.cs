using AniGoldShop.Domain.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AniGoldShop.Domain.Common.Class.Configuration.File
{
    public class UploadFileConfiguration : IUploadFileConfiguration
    {
        public long MaximumSmallFileSize { get; set; }
    }
}
