using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace DemoServerApplication.Configuration
{
    public class CredentialHolder
    {
        private BitmapImage _picture = null;

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public byte[] PictureBytes { get; set; }
        public string[] Roles { get; set; }
        public string Department { get; set; }
        public string CardId { get; set; }
        public DateTime ExpiryDate { get; set; }

        public CredentialHolder Clone(bool includePicture = true)
        {
            return new CredentialHolder() {Id = Id, Name = Name, Password = Password, PictureBytes = PictureBytes, Roles = Roles, Department = Department, CardId = CardId, ExpiryDate = ExpiryDate};
        }
        public static CredentialHolder CreateTemplate()
        {
            CredentialHolder _credentialHolder = new CredentialHolder()
            {
                Id = Guid.NewGuid(),
                Department = "Development",
                CardId = ((int)Math.Round(new Random((int)DateTime.UtcNow.Ticks).NextDouble() * 100000)).ToString(),
                ExpiryDate = DateTime.UtcNow + new TimeSpan(365, 0, 0, 0, 0),
                Roles = new string[] { "Employee", "Authority Level " + ((int)Math.Round(new Random((int)DateTime.UtcNow.Ticks).NextDouble() * 10)).ToString() }
            };

            using (var memStream = new MemoryStream())
            {
                var bmp = new Bitmap(1, 1);
                bmp.Save(memStream, ImageFormat.Jpeg);
                bmp.Dispose();
                _credentialHolder.PictureBytes = memStream.ToArray();
            }
            return _credentialHolder;
        }
        public void SetTestPicture()
        {
            using (var defaultimagestream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoServerApplication.Resources.CredentialHolder.bmp"))
            using (var memStream = new MemoryStream())
            using (var bmp = new Bitmap(defaultimagestream))
            {
                var g = Graphics.FromImage(bmp);
                g.DrawString(this.Name, new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold), Brushes.White, new RectangleF(10, bmp.Height/2, bmp.Width-10, bmp.Height/2 -1));
                bmp.Save(memStream, ImageFormat.Jpeg);
                this.PictureBytes = memStream.ToArray();
            }
        }
        public BitmapImage GetPicture()
        {
            if (_picture == null && PictureBytes != null && PictureBytes.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(PictureBytes))
                    {
                        _picture = new BitmapImage();
                        _picture.BeginInit();
                        _picture.StreamSource = ms;
                        _picture.CacheOption = BitmapCacheOption.OnLoad;
                        _picture.EndInit();
                        _picture.Freeze();
                    }
                }
                catch (Exception)
                {
                }
            }
            return _picture;
        }

        public override bool Equals(object obj)
        {
            CredentialHolder compareCredentialHolder = obj as CredentialHolder;
            if (compareCredentialHolder != null)
            {
                return compareCredentialHolder.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
