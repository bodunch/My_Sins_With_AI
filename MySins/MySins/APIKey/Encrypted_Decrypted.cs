using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MySins.APIKey
{
    internal class Encrypted_Decrypted
    {
        readonly static byte[] entropy = {};

        public static string Encryption(string txt)
        {
            byte[] bytesOriginalTxt = Encoding.Unicode.GetBytes(txt);
            byte[] encryptedTxt = ProtectedData.Protect(bytesOriginalTxt, entropy, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encryptedTxt);
        }

        public static string Decryption(string txt)
        {
            byte[] encryptedTxt = Convert.FromBase64String(txt);
            byte[] originalTxt = ProtectedData.Unprotect(encryptedTxt, entropy, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(originalTxt);
        }
    }
}
