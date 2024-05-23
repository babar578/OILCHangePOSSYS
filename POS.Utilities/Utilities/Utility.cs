using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Net.Mime;
using POS.Utilities.ViewModel;

namespace POS.Utilities.Utilities
{
    public static class Utility
    {
        private static readonly string encryptDecryptKey = "eCom924(";



        public static string Encrypt(string input)
        {
            string encryptString = string.Empty;
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(input);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptDecryptKey));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                tdes.Clear();

                encryptString = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
            }
            return encryptString;
        }
        public static string Decrypt(string input)
        {
            string decryptString = string.Empty;
            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(input);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptDecryptKey));
                hashmd5.Clear();


                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                decryptString = UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
            }
            return decryptString;
        }
        public static bool SendEmail(string toAddress, string messageText, string subject, List<ImageViewModel> images)
        {
            bool returnValue = false;

            string senderEmail = "";
            string senderPassword = "$$NoUser@@123#";

            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    AlternateView altView = AlternateView.CreateAlternateViewFromString(messageText, null, MediaTypeNames.Text.Html);

                    foreach (var x in images)
                    {
                        LinkedResource logo = new LinkedResource(x.ImagePath, MediaTypeNames.Image.Jpeg);
                        logo.ContentId = x.ImageName;
                        altView.LinkedResources.Add(logo);
                    }
                    // main header Images end


                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.Subject = subject;
                    //mailMessage.Body = messageText;

                    mailMessage.AlternateViews.Add(altView);

                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(toAddress));

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.dnbproject1.com";
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential(senderEmail, senderPassword);
                    //NetworkCred.UserName = email;
                    //NetworkCred.Password = "Oculus44$";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 25;

                    smtp.Send(mailMessage);
                    returnValue = true;
                }
                catch (Exception)
                {
                    throw;
                }
                return returnValue;
            }
        }



    }
}
