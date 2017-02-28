using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace Security
{
    public sealed class AESStringEncryptor
    {
        public enum CryptoType { Encrypt, Decrypt };

        SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        public string newAESKey()
        {
            return Convert.ToBase64String(new AesCryptoServiceProvider().Key);
        }

        public string newAESSalt()
        {
            return Convert.ToBase64String(new AesCryptoServiceProvider().IV);
        }

        /// <summary>
        /// Get a SHA256 hashed key from input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected byte[] getKey(string input)
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
        /// <summary>
        /// Get a MD5 hashed salt from input string which will used for adding encryption complexity
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected byte[] getSalt(string input)
        {
            return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        public string AES_Crypto(CryptoType type, string key, string input)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            using (AesCryptoServiceProvider aesObj = new AesCryptoServiceProvider())
            {
                ICryptoTransform transformer;
                byte[] bytes_input;
                byte[] bytes_output;
                string output;

                aesObj.Key = getKey(key);
                aesObj.IV = getSalt(key);

                if(type == CryptoType.Encrypt)
                {
                    transformer = aesObj.CreateEncryptor();
                    bytes_input = Encoding.UTF8.GetBytes(input);
                    bytes_output = transformer.TransformFinalBlock(bytes_input, 0, bytes_input.Length);
                    output = Convert.ToBase64String(bytes_output);
                }
                else
                {
                    transformer = aesObj.CreateDecryptor();
                    bytes_input = Convert.FromBase64String(input);
                    bytes_output = transformer.TransformFinalBlock(bytes_input, 0, bytes_input.Length);
                    output = Encoding.UTF8.GetString(bytes_output);
                }
                return output;
            }
        }
    }
}
