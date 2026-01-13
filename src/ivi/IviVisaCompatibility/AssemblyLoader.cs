using System.Reflection;

namespace Ivi.VisaNet;

/// <summary>   An assembly loader. </summary>
/// <remarks>   2026-01-12. </remarks>
public class AssemblyLoader : IDisposable
{
    private AppDomain? _dom;
    private Assembly? _loadedAssembly;

    /// <summary>   Load an assembly from the specified path. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="assemblyPath"> Full pathname of the assembly file. </param>
    /// <returns>   Loaded assembly. </returns>
    public Assembly? LoadAssembly( string assemblyPath )
    {
        this._dom = AppDomain.CreateDomain( "some" );
        AssemblyName assemblyName = new()
        {
            CodeBase = Path.GetFullPath( assemblyPath )
        };
        this._loadedAssembly = this._dom.Load( assemblyName );
        return this._loadedAssembly;
    }

    /// <summary>   Unload assembly. </summary>
    /// <remarks>   2026-01-12. </remarks>
    public void UnloadAssembly()
    {
        if ( this._dom != null )
        {
            AppDomain.Unload( this._dom );
            this._dom = null;
            this._loadedAssembly = null;
            Console.WriteLine( "AppDomain unloaded." );
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks>   2026-01-12. </remarks>
    public void Dispose()
    {
        this.UnloadAssembly();
        GC.SuppressFinalize( this );
    }
}
