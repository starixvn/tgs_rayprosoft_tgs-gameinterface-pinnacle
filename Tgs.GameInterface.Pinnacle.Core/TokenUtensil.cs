namespace Tgs.GameInterface.Pinnacle.Core;

internal class TokenUtensil : ITokenUtensil
{
    private readonly ISystemClock _systemClock;
    private readonly ICryptographyUtensil _utensil;

    public TokenUtensil(
        ISystemClock systemClock,
        ICryptographyUtensil utensil)
    {
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
    }

    public string GenerateToken(string agentCode, string agentKey, string secretKey)
    {
        var currentTime = _systemClock.CurrentTime;
        string timeStamp = currentTime.ToUnixTimeMilliseconds().ToString();
        string hashToken = _utensil.Md5Encrypt(agentCode + timeStamp + agentKey);
        string tokenPayLoad = string.Format("{0}|{1}|{2}", agentCode, timeStamp, hashToken);
        return _utensil.AesEncrypt(tokenPayLoad, secretKey);
    }
}
