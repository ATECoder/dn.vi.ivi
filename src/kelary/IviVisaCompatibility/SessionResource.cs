namespace Ivi.VisaNet;
public class SessionResource
{
    /// <summary>   Attempts to enumerate resources. </summary>
    /// <remarks>   2025-09-09. </remarks>
    /// <param name="filter">   Specifies the filter. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryEnumerateResources( string filter )
    {
        Ivi.VisaNet.GacLoader.LoadInstalledVisaAssemblies( true );
        List<string>? resources = [.. Ivi.Visa.GlobalResourceManager.Find( filter )];
        return resources?.Count > 0;
    }
}
