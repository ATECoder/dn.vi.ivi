namespace Ivi.VisaNet;

public class SessionResource
{
    /// <summary>   Attempts to enumerate resources. </summary>
    /// <remarks>   2025-09-09. </remarks>
    /// <param name="filter">   Specifies the filter. </param>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public static bool TryEnumerateResources( string filter )
    {
        List<string>? resources = [.. Ivi.Visa.GlobalResourceManager.Find( filter )];
        return resources?.Count > 0;
    }
}
