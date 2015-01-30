namespace Encryptions
{
  using System;
  using System.IO;
  using System.Security.Cryptography;
  using System.Text;

  public static class UnityEncryption
  {
    public static string EncryptOld(string item)
    {
      byte[] dataToEncrypt = Encoding.Unicode.GetBytes(item);
      byte[] encryptedData;

      CspParameters csp = new CspParameters();
      csp.KeyContainerName = "MyKeyContainer";

      csp.Flags = CspProviderFlags.UseMachineKeyStore;
        
      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

      encryptedData = rsa.Encrypt(dataToEncrypt, false);

      return Convert.ToBase64String(encryptedData);
    }

    public static string DecryptOld(string item)
    {
      
      byte[] dataToDecrypt = Convert.FromBase64String(item);
      byte[] decryptedData;

      CspParameters csp = new CspParameters();
      csp.KeyContainerName = "MyKeyContainer";

      csp.Flags = CspProviderFlags.UseMachineKeyStore;

      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

      decryptedData = rsa.Decrypt(dataToDecrypt, false);

      return Encoding.Unicode.GetString(decryptedData);
    }

    public static string Encrypt(string clearText)
    {
      string EncryptionKey = clearText;
      byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

      using (Rijndael encryptor = Rijndael.Create())
      {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x58, 0x76, 0x61, 0x6e, 0x66, 0x4d, 0x79, 0x64, 0x76, 0x65, 0x64, 0x65, 0x42 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        encryptor.Padding = PaddingMode.PKCS7;
        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
          {
            cs.Write(clearBytes, 0, clearBytes.Length);
          }
          clearText = Convert.ToBase64String(ms.ToArray());
        }
      }
      return clearText;
    }

    public static string Decrypt(string cipherText, string passPhrase)
    {
        string EncryptionKey = passPhrase;
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Rijndael encryptor = Rijndael.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x58, 0x76, 0x61, 0x6e, 0x66, 0x4d, 0x79, 0x64, 0x76, 0x65, 0x64, 0x65, 0x42 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            encryptor.Padding = PaddingMode.PKCS7;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;  
    }
  }
}
