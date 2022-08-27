using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

namespace Apps.Models
{
    public class General
    {
        public static Frame Button_With_Icon_And_Text_Of_List(string texto,
            int textoMaxLength,
            string textoFontFamily,
            double buttonHeight,
            bool isClipedToBounds,
            float cornerRadius,
            string backGroundHexaColor,
            string textColor,
            double fontSizeDbl)
        {
            var OnPlatformDic = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"];
            var fontFamily = OnPlatformDic.Platforms.FirstOrDefault((arg) => arg.Platform.FirstOrDefault() == Device.RuntimePlatform).Value;
            Frame frm_not = new Frame()
            {
                CornerRadius = cornerRadius,
                IsClippedToBounds = isClipedToBounds,
                BackgroundColor = Color.FromHex(backGroundHexaColor),
                HeightRequest = buttonHeight,
                Padding = 0
            };

            Label btn = new Label()
            {
                HeightRequest = buttonHeight,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions= LayoutOptions.Center,
                Padding = new Thickness(10, 5,5,10)
            };

            var fString = new FormattedString();
            fString.Spans.Add(new Span()
            {
                Text = ((char)0xf0f3).ToString(),
                FontSize = fontSizeDbl,
                TextColor = Color.White,
                FontFamily = fontFamily.ToString()
            });

            fString.Spans.Add(new Span()
            {
                Text = " " + (texto.Length > textoMaxLength ? texto.Substring(0, textoMaxLength - 1) + "..." : texto),
                FontSize = fontSizeDbl,
                TextTransform = TextTransform.None,
                TextColor = Color.FromHex(textColor),
                FontFamily = textoFontFamily
            });

            btn.FormattedText = fString;
            frm_not.Content = btn;
            return frm_not;
        }
 
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static Stream ByteArrayToStream(byte[] myByteArray)
        {
            MemoryStream stream = new MemoryStream(myByteArray);
            return stream;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
    public static class Extensions
    {
        public static StringContent AsJson(this object o)
            => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
    }


}