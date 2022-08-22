using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.Common.Helper
{
    public class FileUploadFormat
    {
        string _tmaddress = "";
        string _smallAddress = "";
        public IFormFile File { get; set; }

        public string Name { get; set; }

        public string BaseAddress { get; set; }

        public string TempAddress
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_tmaddress))
                    return _tmaddress;
                else
                    return Address;


            }
            set
            {
                _tmaddress = value;
            }
        }

        public string Address { get; set; }

        public string SmallAddress
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_smallAddress))
                    return _smallAddress;
                else
                    return Address;


            }
            set
            {
                _smallAddress = value;
            }
        }

        public int MaxSize { get; set; }

        public string[] SupportetTypes { get; set; }

        public string FileType
        {
            get
            {
                return System.IO.Path.GetExtension(File.FileName).Substring(1).ToLower();
            }
        }

        public bool HasSmall { get; set; }
        public bool IsResize { get; set; }

        public Size SmallSize { get; set; }

        public Size CorrectSize { get; set; }

        public bool HasWatermark
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WatermarkAddress))
                    return false;
                else
                    return true;
            }
        }

        public string WatermarkAddress { get; set; }

        public bool HasWatermarkSmall
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WatermarkAddressSmall))
                    return false;
                else
                    return true;
            }
        }

        public string WatermarkAddressSmall { get; set; }
    }
}
