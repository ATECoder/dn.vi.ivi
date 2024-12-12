using System.Collections.Generic;

namespace Ivi.VisaNet;
public class SessionResource
{
    public static bool TryEnumerateResources( string filter )
    {
        Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies();
        List<string>? resources = new( Ivi.Visa.GlobalResourceManager.Find( filter ) );
        return resources?.Count > 0;
    }

}
