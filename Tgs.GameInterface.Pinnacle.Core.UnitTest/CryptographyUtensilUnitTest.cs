namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class CryptographyUtensilUnitTest
{
    [Fact]
    public void Aes_Decrypt()
    {
        var utensil = new CryptographyUtensil();
        var secretKey = "zenDIdY7Knuzmeen";
        var encrypt = "JgsztSWfevO/V1aRMy9J9WF6een19EGp1CbSRJSSDmtLZ3lfDHnM767ylwPhVBcsIzfWxXQw4D/vo44Z1L6rrw==";
        var decrypt = utensil.AesDecrypt(encrypt, secretKey);

        Assert.Equal("V0201|1684396137576|ed7d02f131b567f6495290b1b96b7793", decrypt);
    }

    [Fact]
    public void Aes_Encrypt()
    {
        var utensil = new CryptographyUtensil();
        var agentCode = "V0201";
        var timeStamp = 1684396137576;
        var secretKey = "zenDIdY7Knuzmeen";
        var hashToken = "ed7d02f131b567f6495290b1b96b7793";
        string tokenPayLoad = string.Format("{0}|{1}|{2}", agentCode, timeStamp, hashToken);
        var encrypt = utensil.AesEncrypt(tokenPayLoad, secretKey);

        Assert.Equal("JgsztSWfevO/V1aRMy9J9WF6een19EGp1CbSRJSSDmtLZ3lfDHnM767ylwPhVBcsIzfWxXQw4D/vo44Z1L6rrw==", encrypt);
    }

    [Fact]
    public void Md5_Encrypt()
    {
        var utensil = new CryptographyUtensil();
        var agentCode = "V0201";
        var timeStamp = 1684396137576;
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var source = agentCode + timeStamp + agentKey;
        var encrypt = utensil.Md5Encrypt(source);

        Assert.Equal("ed7d02f131b567f6495290b1b96b7793", encrypt);
    }
}
