using System.Security.Cryptography;
using System.Text;

namespace Tgs.GameInterface.Pinnacle.Core;

internal class CryptographyUtensil : ICryptographyUtensil
{
    public string AesDecrypt(
        string source,
        string secretKey)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException("this source is empty.");
        }

        var toEncryptArray = Convert.FromBase64String(source);

        var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(secretKey);
        aes.IV = "RandomInitVector"u8.ToArray();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var crypto = aes.CreateDecryptor();

        var resultArray = crypto.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Encoding.UTF8.GetString(resultArray);
    }

    public string AesEncrypt(
        string source,
        string secretKey)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException("this source is empty.");
        }

        var toEncryptArray = Encoding.UTF8.GetBytes(source);

        var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(secretKey);
        aes.IV = "RandomInitVector"u8.ToArray();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var crypto = aes.CreateEncryptor();

        var resultArray = crypto.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public string Md5Encrypt(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            throw new ArgumentException("this source is empty.");
        }

        var toEncryptArray = Encoding.UTF8.GetBytes(source);

        var hash = MD5.HashData(toEncryptArray);

        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }
}
