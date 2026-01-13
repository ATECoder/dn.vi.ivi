using System.Reflection;
using System.Text;
using Ivi.Visa.ConflictManager;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1305  // Specify IFormatProvider

namespace Ivi.VisaNet;

public static partial class GacLoader
{
    /// <summary>   Parse vendor name. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <returns>   A string. </returns>
    public static string ParseVendorName( Assembly installedAssembly )
    {
        if ( installedAssembly is null ) throw new ArgumentNullException( nameof( installedAssembly ), $"{nameof( installedAssembly )} cannot be null." );
        string vendorName = installedAssembly.FullName?.Split( ',' )[0] ?? "UnknownVendor";
        if ( vendorName.Contains( '.' ) )
            vendorName = vendorName.Split( '.' )[0];
        return vendorName;
    }

    /// <summary>   Query if 'typeName' exists in the 'Ivi.Visa' assembly. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="assembly"> The assembly. </param>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   True if type exists, false if not. </returns>
    public static bool IsTypeExists( System.Reflection.Assembly? assembly, string typeName )
    {
        if ( assembly is null ) throw new ArgumentNullException( nameof( assembly ), $"{nameof( assembly )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( typeName ) ) throw new ArgumentException( $"{nameof( typeName )} cannot be null or empty.", nameof( typeName ) );
        _ = System.Reflection.Assembly.LoadFrom( assembly.Location );
        Type? type = assembly.GetType( typeName, false, true );
        return type is not null;
    }

    /// <summary>   Query if 'typeName' exists in the 'Ivi.Visa' assembly. </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="typeName"> Name of the type. </param>
    /// <returns>   True if type exists, false if not. </returns>
    public static bool IsIviVisaTypeExists( string typeName )
    {
        if ( string.IsNullOrWhiteSpace( typeName ) ) throw new ArgumentException( $"{nameof( typeName )} cannot be null or empty.", nameof( typeName ) );
        System.Reflection.Assembly? assembly = GacLoader.GetVisaNetShareComponentsAssembly()
            ?? throw new InvalidOperationException( "VISA.NET Shared Components assembly is not loaded." );
        return GacLoader.IsTypeExists( assembly, typeName );
    }

    /// <summary>   Query if the specific interface is implemented in 'visaSession' . </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="visaSession">      The visa session. </param>
    /// <param name="interfaceName">    Name of the interface. </param>
    /// <returns>   True if interface implemented, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsInterfaceImplemented( Ivi.Visa.IVisaSession visaSession, string interfaceName )
    {
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        if ( GacLoader.IsIviVisaTypeExists( interfaceName ) )
        {
            return visaSession.GetType().GetInterfaces().Any( i => i.FullName == interfaceName || i.Name == interfaceName );
        }
        else
            throw new InvalidOperationException( $"The '{interfaceName}' type does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
    }

    /// <summary>   Query if 'visaSession' implements the specified interface. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="visaSession">      The visa session. </param>
    /// <param name="interfaceName">    Name of the interface. </param>
    /// <param name="details">          [out] The details. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static bool IsInterfaceImplemented( Ivi.Visa.IVisaSession visaSession, string interfaceName, out string details )
    {
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        bool reply = false;
        StringBuilder sb = new();

        if ( GacLoader.IsIviVisaTypeExists( interfaceName ) )
        {
            reply = visaSession.GetType().GetInterfaces().Any( i => i.FullName == interfaceName || i.Name == interfaceName );
            _ = sb.Append( reply
                ? $"\tis"
                : $"\tis not" );
            _ = sb.Append( $" a '{interfaceName}'." );
        }
        else
            _ = sb.Append( $"\tThe '{interfaceName}' type does not exist in '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );

        details = sb.ToString();
        return reply;
    }


    /// <summary>   Query if the specific interface is implemented in 'visaSession' . </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <param name="interfaceName">        Name of the interface. </param>
    /// <returns>   True if interface implemented, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsInterfaceImplemented( System.Reflection.Assembly? installedAssembly, Ivi.Visa.IVisaSession visaSession, string interfaceName )
    {
        if ( installedAssembly is null ) throw new ArgumentNullException( nameof( installedAssembly ), $"{nameof( installedAssembly )} cannot be null." );
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        if ( GacLoader.IsTypeExists( installedAssembly, interfaceName ) )
        {
            return visaSession.GetType().GetInterfaces().Any( i => i.FullName == interfaceName || i.Name == interfaceName );
        }
        else
            throw new InvalidOperationException( $"The type '{interfaceName}' does not exist in '{installedAssembly.FullName}'." );
    }

    /// <summary>   Query if 'visaSession' implements the specified interface. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="ArgumentException">            Thrown when one or more arguments have
    ///                                                 unsupported or illegal values. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <param name="interfaceName">        Name of the interface. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static bool IsInterfaceImplemented( System.Reflection.Assembly? installedAssembly, Ivi.Visa.IVisaSession visaSession, string interfaceName, out string details )
    {
        if ( installedAssembly is null ) throw new ArgumentNullException( nameof( installedAssembly ), $"{nameof( installedAssembly )} cannot be null." );
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        bool reply = false;
        StringBuilder sb = new();

        string vendorName = GacLoader.ParseVendorName( installedAssembly );
        interfaceName = interfaceName.Replace( "Keysight", vendorName );
        try
        {
            if ( GacLoader.IsTypeExists( installedAssembly, interfaceName ) )
            {
                reply = visaSession.GetType().GetInterfaces().Any( i => i.FullName == interfaceName || i.Name == interfaceName );
                _ = sb.Append( reply
                    ? $"\tis"
                    : $"\tis not" );
                _ = sb.Append( $" a '{interfaceName}'." );
            }
            else
                _ = sb.Append( $"\tthe '{interfaceName}' type does not exist in '{installedAssembly.FullName}'." );
        }
        catch ( Exception ex )
        {
            _ = sb.Append( $"\t*** Exception handling '{interfaceName}':\n\t\t{ex.Message}." );
        }
        details = sb.ToString();
        return reply;
    }

    /// <summary>   Query if this object is vendor interface implemented. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <param name="keysightTypeName">     The name of the type assuming a Keysight implementation
    ///                                     where the vendor name is replaced by the parsed vendor name
    ///                                     of the installed assembly. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>   True if vendor interface implemented, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsVendorInterfaceImplemented( System.Reflection.Assembly? installedAssembly, Ivi.Visa.IVisaSession visaSession,
        string keysightInterfaceName, out string details )
    {
        if ( installedAssembly is null ) throw new ArgumentNullException( nameof( installedAssembly ), $"{nameof( installedAssembly )} cannot be null." );
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( keysightInterfaceName ) ) throw new ArgumentException( $"{nameof( keysightInterfaceName )} cannot be null or empty.", nameof( keysightInterfaceName ) );

        string vendorName = GacLoader.ParseVendorName( installedAssembly );
        string vendorInterfaceName = keysightInterfaceName.Replace( "Keysight", vendorName );
        return IsInterfaceImplemented( installedAssembly, visaSession, vendorInterfaceName, out details );
    }

    /// <summary>
    /// Query if this session is an instance of the type name parsed for the specific installed
    /// assembly.
    /// </summary>
    /// <remarks>   2025-09-16. </remarks>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <param name="typeName">             The name of the type. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>   True if instance of type, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsInstanceOfType( Assembly installedAssembly, Ivi.Visa.IVisaSession visaSession, string typeName, out string details )
    {
        bool reply = false;
        try
        {
            Type? candidateType = installedAssembly.GetType( typeName, false, true );
            if ( candidateType is null )
            {
                details = $"\tThe type '{typeName}' does not exist in the assembly '{installedAssembly.FullName}'.";
                return false;
            }
            reply = candidateType.IsInstanceOfType( visaSession );
            details = reply
                ? $"\tis a '{typeName}'."
                : $"\tis not a '{typeName}'.";
        }
        catch ( Exception ex )
        {
            details = $"\t*** Exception handling '{typeName}':\n\t\t{ex.Message}.";
        }
        return reply;
    }

    /// <summary>
    /// Query if this session is an instance of the type name parsed for the specific installed
    /// assembly.
    /// </summary>
    /// <remarks>   2025-09-16. </remarks>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <param name="keySightTypeName">     The name of the type assuming a Keysight implementation
    ///                                     where the vendor name is replaced by the parsed vendor name
    ///                                     of the installed assembly. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>   True if instance of type, false if not. </returns>
    [CLSCompliant( false )]
    public static bool IsInstanceOfVendorType( Assembly installedAssembly, Ivi.Visa.IVisaSession visaSession, string keySightTypeName, out string details )
    {
        bool reply = false;
        string vendorName = GacLoader.ParseVendorName( installedAssembly );
        string vendorTypeName = keySightTypeName.Replace( "Keysight", vendorName );
        try
        {
            Type? candidateType = installedAssembly.GetType( vendorTypeName, false, true );
            if ( candidateType is null )
            {
                details = $"\tThe type '{vendorTypeName}' does not exist in the assembly '{installedAssembly.FullName}'.";
                return false;
            }
            reply = candidateType.IsInstanceOfType( visaSession );
            details = reply
                ? $"\tis a '{vendorTypeName}'."
                : $"\tis not a '{vendorTypeName}'.";
        }
        catch ( Exception ex )
        {
            details = $"\t*** Exception handling '{vendorTypeName}':\n\t\t{ex.Message}.";
        }
        return reply;
    }

    /// <summary>   Validates the visa session interface. </summary>
    /// <remarks>   2025-08-18. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="visaSession">      The visa session. </param>
    /// <param name="interfaceName">    Name of the interface. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static string ValidateVisaSessionInterface( Ivi.Visa.IVisaSession visaSession, string interfaceName )
    {
        StringBuilder sb = new();
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( interfaceName ) ) throw new ArgumentException( $"{nameof( interfaceName )} cannot be null or empty.", nameof( interfaceName ) );
        if ( GacLoader.IsIviVisaTypeExists( interfaceName ) )
        {
            // The type exists in the assembly, so we can use it.
            _ = sb.Append( visaSession.GetType().GetInterfaces().Any( interfaceType => interfaceType.FullName == interfaceName || interfaceType.Name == interfaceName )
                ? $"\tis"
                : $"\tis not" );
            _ = sb.AppendLine( $" a '{interfaceName}'." );
        }
        else
        {
            // The type does not exist in the assembly.
            _ = sb.AppendLine( $"\tThe type '{interfaceName}' does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
        }
        return sb.ToString();
    }

    /// <summary>   Validates the visa session type. </summary>
    /// <remarks>   2025-09-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <param name="visaSession">  The visa session. </param>
    /// <param name="typeName">     Name of the type. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static string ValidateVisaSessionType( Ivi.Visa.IVisaSession visaSession, string typeName )
    {
        StringBuilder sb = new();
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );
        if ( string.IsNullOrWhiteSpace( typeName ) ) throw new ArgumentException( $"{nameof( typeName )} cannot be null or empty.", nameof( typeName ) );
        if ( GacLoader.IsIviVisaTypeExists( typeName ) )
        {
            // The type exists in the assembly, so we can use it.
            _ = sb.Append( visaSession.GetType().GetNestedTypes().Any( nestedType => nestedType.FullName == typeName || nestedType.Name == typeName )
                ? $"\tis"
                : $"\tis not" );
            _ = sb.AppendLine( $" a '{typeName}'." );
        }
        else
        {
            // The type does not exist in the assembly.
            _ = sb.AppendLine( $"\tThe type '{typeName}' does not exist in the namespace '{nameof( Ivi )}.{nameof( Ivi.Visa )}'." );
        }
        return sb.ToString();
    }

    /// <summary>   Identify visa session. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="visaSession">  The visa session. </param>
    /// <returns>   A string listing the interfaces implemented by the visa session. </returns>
    [CLSCompliant( false )]
    public static string BuildSessionInterfaceImplementationReport( Ivi.Visa.IVisaSession? visaSession )
    {
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );

        System.Text.StringBuilder sb = new();

        _ = sb.AppendLine( $"Identifying session implementations by type names:" );

        string[] interfaces = ["Ivi.Visa.IVisaSession", "Ivi.Visa.IMessageBasedSession",
            "Ivi.Visa.ITcpipSession", "Ivi.Visa.ITcpipSession2", "Ivi.Visa.ITcpipSocketSession", "Ivi.Visa.ITcpipSocketSession2",
            "Ivi.Visa.IGpibInterfaceSession", "Ivi.Visa.IGpibSession",
            "Ivi.Visa.INativeVisaSession",
            "Ivi.Visa.IPxiBackplaneSession", "Ivi.Visa.IPxiMemorySession", "Ivi.Visa.IPxiSession", "Ivi.Visa.IPxiSession2",
            "Ivi.Visa.IRegisterBasedSession",
            "Ivi.Visa.ISerialSession",
            "Ivi.Visa.IVxiBackplaneSession", "Ivi.Visa.IVxiMemorySession", "Ivi.Visa.IVxiSession"
            ];
        foreach ( string interfaceName in interfaces )
        {
            _ = sb.AppendLine( GacLoader.IsInterfaceImplemented( visaSession, interfaceName, out string details ) ? details : details );
        }

# if false
        _ = sb.Append( visaSession is Ivi.Visa.IVisaSession
            ? $"\tis"
            : $"\tis not" );
        _ = sb.AppendLine( $" a '{nameof( Ivi )}.{nameof( Ivi.Visa )}.{nameof( Ivi.Visa.IVisaSession )}'." );
#endif
        return sb.ToString();

    }

    /// <summary>   Builds session types report. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="visaSession">          The visa session. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static string BuildSessionTypesReport( System.Reflection.Assembly? installedAssembly, Ivi.Visa.IVisaSession? visaSession )
    {
        if ( installedAssembly is null ) throw new ArgumentNullException( nameof( installedAssembly ), $"{nameof( installedAssembly )} cannot be null." );
        if ( visaSession is null ) throw new ArgumentNullException( nameof( visaSession ), $"{nameof( visaSession )} cannot be null." );

        System.Text.StringBuilder sb = new();

        string[] vendorTypeNames = ["Keysight.Visa.MessageBasedSession",
            "Keysight.Visa.GpibInterfaceSession", "Keysight.Visa.GpibSession",
            "Keysight.Visa.PxiBackplaneSession", "Keysight.Visa.PxiMemorySession", "Keysight.Visa.PxiSession",
            "Keysight.Visa.RegisterBasedSession",
            "Keysight.Visa.SerialSession",
            "Keysight.Visa.TcpipSession", "Keysight.Visa.TcpipSocketSession",
            "Keysight.Visa.UsbSession",
            "Keysight.Visa.VisaSession",
            "Keysight.Visa.VxiBackplaneSession", "Keysight.Visa.VxiMemorySession", "Keysight.Visa.VxiSession"];

        string[] vendorInterfaceNames = ["Keysight.Visa.IKeysightNativeVisaSession"];

        _ = sb.AppendLine( $"\nIdentifying session types by vendor type names:" );

        foreach ( string vendorTypeName in vendorTypeNames )
        {
            _ = sb.AppendLine( GacLoader.IsInstanceOfVendorType( installedAssembly, visaSession, vendorTypeName, out string details ) ? details : details );
        }

        _ = sb.AppendLine( $"\nIdentifying session interface implementations by vendor type names:" );

        foreach ( string vendorInterfaceName in vendorInterfaceNames )
        {
            _ = sb.AppendLine( GacLoader.IsVendorInterfaceImplemented( installedAssembly, visaSession, vendorInterfaceName, out string details ) ? details : details );
        }

        return sb.ToString();
    }

    /// <summary>   Builds session identity report. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="visaSession">  The visa session. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static string BuildSessionIdentityReport( Ivi.Visa.IVisaSession? visaSession )
    {
        StringBuilder sb = new();
        foreach ( Ivi.Visa.ConflictManager.VisaImplementation visaLibrary in new Ivi.Visa.ConflictManager.ConflictManager().GetInstalledVisas( ApiType.DotNet ) )
        {
            try
            {
                string fileName =
#if NET20_OR_GREATER
                visaLibrary.Location.Substring( visaLibrary.Location.IndexOf( ',' ) + 1 );
#else
                visaLibrary.Location[(visaLibrary.Location.IndexOf( ',' ) + 1)..];
#endif
                sb.AppendLine( $"Loading assembly from '{fileName}'..." );
                using AssemblyLoader assemblyLoader = new();
                Assembly? installedAssembly = assemblyLoader.LoadAssembly( visaLibrary );
                if ( installedAssembly is null )
                {
                    sb.AppendLine( $"*** Failed to load assembly from {fileName}." );
                    continue;
                }
                else
                {
                    Version? installedVersion = installedAssembly.GetName().Version;
                    sb.AppendLine( $"\tLoaded {installedAssembly.FullName}." );
                    sb.AppendLine( $"\tVersion: {System.Diagnostics.FileVersionInfo.GetVersionInfo( installedAssembly.Location ).FileVersion}." );
                    sb.AppendLine( GacLoader.BuildSessionTypesReport( installedAssembly, visaSession ) );
                }
            }
            catch ( Exception exception )
            {
                Console.WriteLine( $"\t*** Failed to load assembly {visaLibrary.FriendlyName}: {exception.Message}" );
            }
            sb.AppendLine( $"\tUnloading assembly." );
        }
        return sb.ToString();
    }

    /// <summary>   Identify visa session. </summary>
    /// <remarks>   2026-01-12. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   A string. </returns>
    [CLSCompliant( false )]
    public static Ivi.Visa.IVisaSession? IdentifyVisaSession( string resourceName, out string details )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
        {
            details = $"{nameof( resourceName )} is null or empty or white space.";
            return null;
        }
        Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 );

        details = GacLoader.BuildSessionInterfaceImplementationReport( visaSession );
        return visaSession;
    }

    /// <summary>   Identify visa session. </summary>
    /// <remarks>   2025-09-17. </remarks>
    /// <param name="installedAssembly">    The installed assembly. </param>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <returns>   A string. </returns>
    public static string IdentifyVisaSession( Assembly installedAssembly, string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
            return $"{nameof( resourceName )} is null or empty or white space.";

        System.Text.StringBuilder sb = new();

        using Ivi.Visa.IVisaSession visaSession = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.ExclusiveLock, 2000 );

        _ = sb.AppendLine( $"Identifying '{resourceName}' session interface implementations and vendor types:" );
        _ = sb.AppendLine( GacLoader.BuildSessionInterfaceImplementationReport( visaSession ) );
        _ = sb.AppendLine( GacLoader.BuildSessionTypesReport( installedAssembly, visaSession ) );
        return sb.ToString();
    }
}
