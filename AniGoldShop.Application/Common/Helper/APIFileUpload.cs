using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace AniGoldShop.Application.Common.Helper
{
    public class APIFileUpload
    {
        string _tmaddress = "";
        string _smallAddress = "";
        public IFormFile File { get; set; }

        public string FLAddress { private set; get; }

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
                string f = null;
                try
                {
                    f = System.IO.Path.GetExtension(File.FileName).Substring(1).ToLower();
                }
                catch
                {

                }
                if (string.IsNullOrWhiteSpace(f))
                {
                    return File.ContentType.Split('/')[1];
                }
                else
                {
                    return f;
                }
            }
        }

        public bool HasSmall { get; set; }

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

        public bool SaveJustSmall { get; set; }

        public string AbsolutePath { get; set; }
        public string StaticServeDomain { get; set; }
        private bool IsDevelopment { get; set; }
        private string MiddleAddress { get; set; }
        public APIFileUpload(bool isDevelpMode = false)
        {
            IsDevelopment = isDevelpMode;
            if (IsDevelopment)
            {
                MiddleAddress = "wwwroot";
            }
            else
            {
                MiddleAddress = "wwwroot";

            }
        }

        #region File Uploading Methods

        public FuncResult UploadFile()
        {

            FuncResult _result = new FuncResult();
            string _createdUrl = "";

            try
            {
                if (File != null)
                {

                    if (SupportetTypes != null && !SupportetTypes.Contains(FileType))
                    {
                        _result.Message = "فایل معتبر نمی باشد";
                        _result.Successful = false;
                    }
                    else if (File.Length > 0)
                    {
                        if (File.Length <= MaxSize)
                        {
                            string directory = null;

                            if (string.IsNullOrWhiteSpace(AbsolutePath))
                                directory = Directory.GetCurrentDirectory() + "\\" + MiddleAddress + Address;
                            else
                                directory = AbsolutePath;
                            directory = directory.Replace(@"\", @"/");

                            var fileName = Path.GetFileName(File.FileName);
                            string path = "";

                            if (string.IsNullOrWhiteSpace(AbsolutePath))
                                path = Directory.GetCurrentDirectory() + "\\" + MiddleAddress + Address + "\\" + Name + "." + FileType;
                            else
                                path = AbsolutePath + "/" + Name + "." + FileType;

                            path = path.Replace(@"\", @"/");
                            if (!Directory.Exists(directory))
                            {
                                CreateDirectory(directory);
                            }

                            FLAddress = path;

                            if (HasSmall)
                            {
                                string smalldirectory = null;

                                if (string.IsNullOrWhiteSpace(AbsolutePath))
                                    smalldirectory = Directory.GetCurrentDirectory() + "\\" + MiddleAddress + SmallAddress;
                                else
                                    smalldirectory = AbsolutePath;

                                smalldirectory = smalldirectory.Replace(@"\", @"/");

                                if (Address != SmallAddress && !Directory.Exists(smalldirectory))
                                {
                                    CreateDirectory(smalldirectory);

                                }

                                string smallpath = "";

                                if (string.IsNullOrWhiteSpace(AbsolutePath))
                                    smallpath = Directory.GetCurrentDirectory() + "\\" + MiddleAddress + Address + "\\" + Name + "_sm." + FileType;
                                else
                                    smallpath = AbsolutePath + "/" + Name + "_sm." + FileType;

                                smallpath = smallpath.Replace(@"\", @"/");

                                //WebImage img = new WebImage(File.OpenReadStream());
                                SixLabors.ImageSharp.Formats.IImageEncoder format;
                                using var img = SixLabors.ImageSharp.Image.Load(File.OpenReadStream());

                                if (!SaveJustSmall)
                                {
                                    using FileStream fsn = System.IO.File.Create(path);
                                    img.Save(fsn, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                                    fsn.Flush();
                                    fsn.Close();

                                    if (HasWatermark)
                                    {
                                        AddWaterMark(path, WatermarkAddress, path);
                                    }
                                }

                                //if (img.Width >= 600)
                                //    img.Resize(270, 270);


                                if (SmallSize.Width == 0 && SmallSize.Height == 0)
                                {

                                    if (img.Width > img.Height)
                                    {
                                        if (img.Width > 1200)
                                            SmallSize = new Size(1200, img.Height);
                                        else
                                            SmallSize = new Size(img.Width, img.Height);


                                        if (img.Height > 900)
                                            SmallSize = new Size(SmallSize.Width, 900);
                                        else
                                            SmallSize = new Size(SmallSize.Width, img.Height);

                                    }
                                    if (img.Width < img.Height)
                                    {
                                        if (img.Height > 1200)
                                            SmallSize = new Size(img.Width, 1200);
                                        else
                                            SmallSize = new Size(img.Width, img.Height);


                                        if (img.Width > 900)
                                            SmallSize = new Size(900, SmallSize.Height);
                                        else
                                            SmallSize = new Size(img.Width, SmallSize.Height);

                                    }
                                    if (img.Width == img.Height)
                                    {
                                        if (img.Height > 1200)
                                            SmallSize = new Size(img.Width, 1200);
                                        else
                                            SmallSize = new Size(img.Width, img.Height);


                                        if (img.Width > 1200)
                                            SmallSize = new Size(1200, SmallSize.Height);
                                        else
                                            SmallSize = new Size(img.Width, SmallSize.Height);

                                    }

                                }

                                if (img.Width < img.Height)
                                {
                                    if (SmallSize.Width > SmallSize.Height)
                                    {
                                        var w = SmallSize.Width;
                                        var h = SmallSize.Height;
                                        SmallSize = new Size(h, w);
                                    }
                                }

                                img.Mutate(x => x.Resize(SmallSize.Width, SmallSize.Height, true));


                                using FileStream fs = System.IO.File.Create(smallpath);

                                if (FileType.ToLower() == "png")
                                {
                                    img.Save(fs, new SixLabors.ImageSharp.Formats.Png.PngEncoder()
                                    {
                                        CompressionLevel = SixLabors.ImageSharp.Formats.Png.PngCompressionLevel.BestCompression
                                    });
                                }
                                else
                                {

                                    img.Save(fs, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder()
                                    {
                                    });
                                }

                                fs.Flush();
                                fs.Close();


                                if (HasWatermarkSmall)
                                {
                                    AddWaterMark(smallpath, WatermarkAddressSmall, smallpath);
                                }

                            }
                            else
                            {
                                using FileStream fs = System.IO.File.Create(path);
                                File.CopyTo(fs);
                                fs.Flush();
                                fs.Close();

                                if (HasWatermark)
                                {
                                    AddWaterMark(path, WatermarkAddress, path);
                                }
                            }

                            if (string.IsNullOrWhiteSpace(AbsolutePath))
                                _createdUrl = (!SaveJustSmall)
                                    ? BaseAddress + TempAddress + "/" + Name + "." + FileType
                                    : BaseAddress + SmallAddress + "/" + Name + "_sm." + FileType;
                            else
                                _createdUrl = (!SaveJustSmall)
                                    ? BaseAddress + TempAddress + "/" + Name + "." + FileType
                                    : BaseAddress + SmallAddress + "/" + Name + "_sm." + FileType;

                            if (string.IsNullOrWhiteSpace(AbsolutePath))
                                if (!_createdUrl.StartsWith("/"))
                                    _createdUrl = "/" + _createdUrl;

                            _result.Message = "»وفیقت آمیز بود";
                            _result.Successful = true;
                            _result.Data = _createdUrl;

                        }
                        else
                        {

                            _result.Message = "حجم فایل بیش از " + MaxSize.ToString().Substring(0, MaxSize.ToString().Length - 3) + " می باشد";
                            _result.Successful = false;
                        }
                    }
                    else
                    {
                        _result.Message = "فایل معتبر نمی باشد";
                        _result.Successful = false;
                    }
                }
                else
                {
                    _result.Message = "فایل معتبر نمی باشد";
                    _result.Successful = false;
                }
            }
            catch (Exception ex)
            {
                _result.Message = "خطا";
                _result.Successful = false;
            }


            return _result;

        }


        #endregion


        #region Add WaterMark to Image

        public bool AddWaterMark(string imgAddress, string waterMarkAddress, string saveAddress)
        {

            try
            {

                using (Image image = Image.FromFile(imgAddress))
                using (Image watermarkImage = Image.FromFile(waterMarkAddress))
                using (Graphics imageGraphics = Graphics.FromImage(image))
                using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
                {
                    int x = (image.Width / 2 - watermarkImage.Width / 2);
                    int y = (image.Height / 2 - watermarkImage.Height / 2);
                    watermarkBrush.TranslateTransform(x, y);
                    imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                    image.Save(saveAddress);
                }

                //try
                //{
                //    System.IO.File.Delete(imgAddress);
                //}
                //catch (Exception)
                //{

                //}

                return true;

            }
            catch
            {

                return false;
            }


        }

        #endregion

        private bool CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                var user = WindowsIdentity.GetCurrent().User;
                var userName = user.Translate(typeof(NTAccount));
                var dirInfo = new DirectoryInfo(path);
                var sec = dirInfo.GetAccessControl();
                sec.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(userName,
                                System.Security.AccessControl.FileSystemRights.Modify,
                                System.Security.AccessControl.AccessControlType.Allow)
                                );
                dirInfo.SetAccessControl(sec);
                System.IO.Directory.CreateDirectory(path);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Delete File From Disk

        public bool DeleteFile(string Address, ref string mess)
        {
            try
            {
                string path = "";
                path = Address;


                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    mess = "موفقیت آمیز بود";
                    return true;
                }
                else
                {
                    mess = "فابل مورد نظر وجو ندارد";
                    return true;
                }

            }
            catch (Exception ex)
            {
                mess = "خطا در حذف فایل";
                return false;
            }

        }

        #endregion



    }
}
