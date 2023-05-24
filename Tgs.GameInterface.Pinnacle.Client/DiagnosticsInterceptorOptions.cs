using System.Diagnostics;

namespace Tgs.GameInterface.Pinnacle.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.Pinnacle.Client.DiagnosticsInterceptor");
}
