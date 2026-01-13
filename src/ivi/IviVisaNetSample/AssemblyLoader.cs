using System.Reflection;
#if NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Ivi.VisaNet;

#pragma warning disable CA1305

/// <summary>   An assembly loader. </summary>
/// <remarks>   2026-01-12. </remarks>
public class AssemblyLoader : IDisposable
{
#if NET5_0_OR_GREATER
    private CustomAssemblyLoadContext? _loadContext;
#else
    private AppDomain? _dom;
#endif
    private Assembly? _loadedAssembly;

    /// <summary>   Load an assembly from the specified path. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="assemblyPath"> Full pathname of the assembly file. </param>
    /// <returns>   Loaded assembly. </returns>
    public Assembly? LoadAssembly( string assemblyPath )
    {
        // Ensure the path is absolute
        string absolutePath = Path.GetFullPath( assemblyPath );

#if NET5_0_OR_GREATER
        // Create a new, collectible context for each assembly/plugin you want to unload
        this._loadContext = new CustomAssemblyLoadContext();

        // Load the assembly from the specified file path into the custom context
        this._loadedAssembly = this._loadContext.LoadFromAssemblyPath( absolutePath );

        Console.WriteLine( $"Assembly loaded into context: {this._loadContext.Name}" );
#else
        this._dom = AppDomain.CreateDomain( "some" );
        AssemblyName assemblyName = new()
        {
            CodeBase = assemblyPath
        };
        this._loadedAssembly = this._dom.Load( assemblyName );
#endif
        return this._loadedAssembly;
    }

    /// <summary>   Unload assembly. </summary>
    /// <remarks>   2026-01-12. </remarks>
    public void UnloadAssembly()
    {
#if NET5_0_OR_GREATER
        if ( this._loadContext != null )
        {
            // Initiate the cooperative unloading process
            this._loadContext.Unload();
            this._loadedAssembly = null;

            // Wait for the garbage collection to finalize the unload
            // This is often needed in examples to ensure the unload completes for immediate checks
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine( "Assembly unload initiated and garbage collection run." );
            this._loadContext = null;
        }
#else
        if ( this._dom != null )
        {
            AppDomain.Unload( this._dom );
            this._dom = null;
            this._loadedAssembly = null;
            Console.WriteLine( "AppDomain unloaded." );
        }
#endif
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

#if NET5_0_OR_GREATER
// 1. Define a custom AssemblyLoadContext that is collectible
public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base( isCollectible: true ) { }

    protected override Assembly? Load( AssemblyName assemblyName )
    {
        // This is where you can define custom logic to load dependencies. 
        // For simplicity, we return null to use the default loading behavior for dependencies 
        // that are part of the main application or already in the default context.
        return null;
    }
}
#endif
