namespace Tgs.GameInterface.Pinnacle.Core;

internal interface ICryptographyUtensil
{
    public string AesDecrypt(
        string source,
        string secretKey);

    public string AesEncrypt(
        string source,
        string secretKey);

    public string Md5Encrypt(string source);
}
