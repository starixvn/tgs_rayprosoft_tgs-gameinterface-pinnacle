namespace Tgs.GameInterface.Pinnacle.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
