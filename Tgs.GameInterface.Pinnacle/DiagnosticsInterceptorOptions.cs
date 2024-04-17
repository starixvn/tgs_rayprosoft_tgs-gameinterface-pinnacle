using System.Diagnostics;

namespace Tgs.GameInterface.Pinnacle;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Pinnacle.DiagnosticsInterceptor");
}
