using System.Collections.Generic;

namespace Ivi.VisaNet;
public class SessionResource
{
    public static bool TryEnumerateResources( string filter )
    {
        Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies( true );
        List<string>? resources = [.. Ivi.Visa.GlobalResourceManager.Find( filter )];
        return resources?.Count > 0;
    }
}
