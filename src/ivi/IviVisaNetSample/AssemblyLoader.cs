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

    /// <summary>   Bytes to hexadecimal. </summary>
    /// <remarks>   2024-07-01. </remarks>
    /// <param name="byteArray">    Array of bytes. </param>
    /// <returns>   A string. </returns>
    private static string BytesToHex( byte[] byteArray )
    {
        // Convert byte array to hexadecimal string
        System.Text.StringBuilder hexBuilder = new();
        foreach ( byte b in byteArray )
        {
#if NET5_0_OR_GREATER
            _ = hexBuilder.Append( System.Globalization.CultureInfo.CurrentCulture, $"{b:x2}" ); // X2 ensures two-digit representation
#else
            _ = hexBuilder.Append( $"{b:x2}" ); // X2 ensures two-digit representation
#endif
        }
        return hexBuilder.ToString();
    }

    /// <summary>   Query if this object contains the given str. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="str">          The string. </param>
    /// <param name="substring">    The substring. </param>
    /// <param name="comp">         The component. </param>
    /// <returns>   True if the object is in this collection, false if not. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Usage", "CA2249:Consider using 'string.Contains' instead of 'string.IndexOf'", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Usage", "CA2263:Prefer generic overload when type is known", Justification = "<Pending>" )]
    private static bool Contains( string str, string substring, StringComparison comp )
    {
        if ( substring is null ) throw new ArgumentNullException( nameof( substring ), $"{nameof( substring )} cannot be null." );
        else if ( !Enum.IsDefined( typeof( StringComparison ), comp ) )
            throw new ArgumentException( $"{nameof( comp )} is not a member of {nameof( StringComparison )}", nameof( comp ) );
        return str.IndexOf( substring, comp ) >= 0;
    }

    /// <summary>   Load an assembly from the GAC. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="visaLibrary">  The visa library. </param>
    /// <returns>   Loaded assembly. </returns>
    [CLSCompliant( false )]
    public System.Reflection.Assembly? LoadAssembly( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary )
    {
        string fileName =
#if NET20_OR_GREATER
                visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 );
#else
        visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..];
#endif
        return this.LoadAssembly( new System.Reflection.AssemblyName( fileName ) );
    }

    /// <summary>
    /// Load an assembly from the GAC.
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns>Loaded assembly</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public System.Reflection.Assembly? LoadAssembly( System.Reflection.AssemblyName assemblyName )
    {
        string[] gacPaths =
        [
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\Microsoft.NET\\assembly\\GAC_MSIL\\{assemblyName.Name}",
           $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\assembly\\GAC_MSIL\\{assemblyName.Name}",
        ];

        foreach ( string folder in gacPaths.Where( System.IO.Directory.Exists ) )
        {
            foreach ( string subfolder in System.IO.Directory.EnumerateDirectories( folder ) )
            {
                if ( AssemblyLoader.Contains( subfolder, BytesToHex( assemblyName.GetPublicKeyToken()! ), StringComparison.OrdinalIgnoreCase )
                    && AssemblyLoader.Contains( subfolder, assemblyName.Version!.ToString(), StringComparison.OrdinalIgnoreCase ) )
                {
                    string assemblyPath = System.IO.Path.Combine( subfolder, assemblyName.Name + ".dll" );
                    if ( System.IO.File.Exists( assemblyPath ) )
                        return this.LoadAssembly( assemblyPath );
                }
            }
        }
        throw new System.IO.FileNotFoundException( $"Assembly {assemblyName} not found." );
    }

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
