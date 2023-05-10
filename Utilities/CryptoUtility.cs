using System.IO;
using System.Security.Cryptography;

namespace BloggerApi.Utilities {
  // this is the microsoft recommended implementation, and the source for this implementation.
  // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0
  public abstract class CryptoUtility {
    public static string DecryptCypher(byte[] cypher, byte[] key, byte[] iv) {
      // Create an Aes object
      // with the specified key and IV.
      using var aesAlg = Aes.Create();

      aesAlg.Key = key;
      aesAlg.IV = iv;

      // Create a decryptor to perform the stream transform.
      var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

      // Create the streams used for decryption.
      using var msDecrypt = new MemoryStream(cypher);
      using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
      using var srDecrypt = new StreamReader(csDecrypt);

      var plaintext = srDecrypt.ReadToEnd();

      return plaintext;
    }

    public static byte[] EncryptString(string input, byte[] key, byte[] iv) {
      // Create an Aes object
      // with the specified key and IV.
      using var aesAlg = Aes.Create();

      aesAlg.Key = key;
      aesAlg.IV = iv;

      // Create an encryptor to perform the stream transform.
      var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

      // Create the streams used for encryption.
      using var msEncrypt = new MemoryStream();
      using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
      using (var swEncrypt = new StreamWriter(csEncrypt)) {
        // Write all data to the stream.
        swEncrypt.Write(input);
      }

      var encrypted = msEncrypt.ToArray();

      // Return the encrypted bytes from the memory stream.
      return encrypted;
    }
  }
}