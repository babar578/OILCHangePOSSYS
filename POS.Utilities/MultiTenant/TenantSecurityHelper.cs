using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// Provides encryption/decryption for sensitive tenant data like database passwords
    /// </summary>
    public static class TenantSecurityHelper
    {
        // NOTE: In production, store these keys securely in web.config appSettings or Azure Key Vault
        // These are hardcoded here for demonstration purposes
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("MultiTenant32ByteEncryptionKey"); // Must be 32 bytes for AES-256
        private static readonly byte[] _iv = Encoding.UTF8.GetBytes("16ByteIVForAES!!"); // Must be 16 bytes

        /// <summary>
        /// Encrypts a plain text password using AES encryption
        /// </summary>
        public static string EncryptPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encrypted);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error encrypting password: {ex.Message}");
                throw new Exception("Failed to encrypt password", ex);
            }
        }

        /// <summary>
        /// Decrypts an encrypted password using AES decryption
        /// </summary>
        public static string DecryptPassword(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] cipher = Convert.FromBase64String(cipherText);
                    byte[] decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
            catch (Exception ex)
            {
                // If decryption fails, the password might not be encrypted (backward compatibility)
                System.Diagnostics.Debug.WriteLine($"Error decrypting password: {ex.Message}");
                // Return the original value for backward compatibility
                return cipherText;
            }
        }

        /// <summary>
        /// Checks if a string appears to be encrypted (Base64 format check)
        /// </summary>
        public static bool IsEncrypted(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

