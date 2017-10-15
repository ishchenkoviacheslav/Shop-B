using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace Al_Baraka
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string clearText)
        {
            byte[] symbolsinbyte = new byte[50];
            char[] symbolsinchar = clearText.ToCharArray();
            int maxchar = 0;
            if (clearText.Length < 50)//pass must be less than 50 chars
            {

                for (int i = 0; i < 50; i++)
                {
                    if (i % (symbolsinchar.Length) == 0)//new round
                    {
                        maxchar = 0;
                    }
                    byte original = (byte)symbolsinchar[maxchar];
                    byte code = (byte)symbolsinchar[symbolsinchar.Length - 1 - maxchar];
                    symbolsinbyte[i] = (byte)(original + code);
                    maxchar++;
                }
                char[] encodedCharArray = new char[50];
                for (int i = 0; i < symbolsinbyte.Length; i++)
                {
                    encodedCharArray[i] = (char)symbolsinbyte[i];
                }
                string temp = new string(encodedCharArray);
                return temp;
            }
            else
            {
                return "password must have less than 50 symbols";
            }
            //string EncryptionKey = "abc123";
            //byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(clearBytes, 0, clearBytes.Length);
            //            cs.Dispose();
            //        }
            //        clearText = Convert.ToBase64String(ms.ToArray());
            //    }
            //}
            //return clearText;
        }
        public static string Decrypt(string enteredPass, string encodedPass)
        {
            char[] codedPassCharArray = enteredPass.ToCharArray();
            char[] encodedArray = new char[enteredPass.Length];
            for (int i = 0; i < 50; i++)
            {
                if (codedPassCharArray.Length == i)
                    break;
                byte code = (byte)codedPassCharArray[codedPassCharArray.Length - 1 - i];
                byte coded = (byte)encodedPass[i];
                encodedArray[i] = (char)(byte)(coded - code);
            }
            return new string(encodedArray);
            //string EncryptionKey = "abc123";
            //cipherText = cipherText.Replace(" ", "+");
            //byte[] cipherBytes = Convert.FromBase64String(cipherText);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(cipherBytes, 0, cipherBytes.Length);
            //            cs.Dispose();
            //        }
            //        cipherText = Encoding.Unicode.GetString(ms.ToArray());
            //    }
            //}
            //return cipherText;
        }
    }
}
